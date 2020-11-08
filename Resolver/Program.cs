using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DnsClient;
using DnsClient.Protocol;

namespace Resolver
{
    internal static class Program
    {
        public static async Task Main(string[] args)
        {
            var inputFilename = args[0];
            var rawList = File.ReadAllLines(inputFilename);
            var domains = rawList
                .Where(t => (t.Trim() != "") && !t.StartsWith("#"))
                .Select(t => t.Trim())
                .ToHashSet();
            var usedDomains = new HashSet<string>();
            var tasks = domains
                .Select((domain, index) =>
                {
                    if (usedDomains.Contains(domain))
                    {
                        return (null, -1, null);
                    }

                    usedDomains.Add(domain);

                    Task<ICollection<string>> task = GetMXIpForDomain(domain);
                    return (domain, index, task);
                })
                .Where(t => t.domain != null)
                .ToArray();

            //
            await Task.WhenAll(tasks.Select(t => t.task).ToArray());

            var lines = tasks
                .Select(t =>
                {
                    t.task!.Wait();
                    var ips = t.task.Result;
                    if (!ips.Any())
                    {
                        Console.WriteLine("Domain {0} contains no valid IPs for MX-records", t.domain);
                        return null;
                    }

                    var smtpLine = string.Concat(ips.Select(ip => string.Format(":[{0}]", ip)));
                    return string.Format(
                        "    echo '{0} smtp{1}' >> '/etc/mail/mailertable'",
                        t.domain,
                        smtpLine
                    );
                })
                .Where(t => t != null)
                .Aggregate("", (a, b) => a + " && \\\n" + b);

            File.WriteAllText(args[1], lines);
        }

        private static readonly SemaphoreSlim mutex = new SemaphoreSlim(5, 5);

        private static async Task<ICollection<string>> GetMXIpForDomain(string domain)
        {
            await mutex.WaitAsync();
            try
            {
                return await GetMXIpForDomainInner(domain);
            }
            finally
            {
                mutex.Release();
            }
        }

        private static async Task<ICollection<string>> GetMXIpForDomainInner(string domain)
        {
            var lookup = new LookupClient(new LookupClientOptions()
            {
                Timeout = TimeSpan.FromMinutes(1),
            });
            var result = await DNSRequest(lookup, domain, QueryType.MX);

            Console.WriteLine("{0,-30}:\tGet MX-records", domain);
            var mxDomains = result
                .Answers
                .OfType<MxRecord>()
                .OrderBy(t => t.Preference)
                .Select(t => t.Exchange.Value)
                .ToArray();

            Console.WriteLine("{1,-30}:\tGot {0} MX-records", mxDomains.Length, domain);
            var allIps = new List<string>();
            foreach (var mxDomain in mxDomains)
            {
                result = await DNSRequest(lookup, mxDomain, QueryType.ANY);
                if (result.HasError)
                {
                    Console.WriteLine("{3,-30}:\tCan not get DNS records for {0} from {1}. Error: {2}",
                        mxDomain,
                        lookup.NameServers.First().Address,
                        result.ErrorMessage,
                        domain
                    );
                    continue;
                }

                var mxIps = result
                    .Answers
                    .Select(t =>
                    {
                        return t switch
                        {
                            ARecord r1 => r1.Address.ToString(),
                            AaaaRecord r2 => r2.Address.ToString(),
                            _ => null
                        };
                    })
                    .Where(t => t != null)
                    .ToHashSet();

                if (mxIps.Any())
                {
                    allIps.AddRange(mxIps);
                    continue;
                }

                // https://tools.ietf.org/html/rfc8482
                var rfcRestriction = result
                    .Answers
                    .OfType<HInfoRecord>()
                    .Any(x => x.Cpu.ToUpperInvariant() == "RFC8482");
                DnsResourceRecord soaRaw;
                if (rfcRestriction)
                {
                    result = await DNSRequest(lookup, mxDomain, QueryType.AAAA);
                    var ip_aaaa = result
                        .Answers
                        .OfType<AaaaRecord>()
                        .Select(t => t.Address.ToString());

                    result = await DNSRequest(lookup, mxDomain, QueryType.A);
                    var ip_a = result
                        .Answers
                        .OfType<ARecord>()
                        .Select(t => t.Address.ToString())
                        .Concat(ip_aaaa);

                    if (ip_a.Any())
                    {
                        allIps.AddRange(ip_a);
                        continue;
                    }

                    result = await DNSRequest(lookup, mxDomain, QueryType.SOA);

                    soaRaw = result
                        .Answers
                        .FirstOrDefault(t => t.RecordType == ResourceRecordType.SOA);
                }
                else
                {
                    soaRaw = result
                        .Answers
                        .FirstOrDefault(t => t.RecordType == ResourceRecordType.SOA);
                }

                if (!(soaRaw is SoaRecord soa))
                {
                    continue;
                }

                var soaResult = await DNSRequest(lookup, soa.MName, QueryType.ANY);

                var soaARecord = soaResult
                    .Answers
                    .Select(t =>
                    {
                        return t switch
                        {
                            ARecord r1 => r1.Address.ToString(),
                            AaaaRecord r2 => r2.Address.ToString(),
                            _ => null
                        };
                    })
                    .FirstOrDefault(t => t != null);
                if (soaARecord == null)
                {
                    continue;
                }

                var lookupLocal = new LookupClient(new LookupClientOptions(IPAddress.Parse(soaARecord))
                {
                    Timeout = TimeSpan.FromMinutes(1),
                });
                result = await DNSRequest(lookupLocal, mxDomain, QueryType.ANY);

                mxIps = result
                    .Answers
                    .Select(t =>
                    {
                        return t switch
                        {
                            ARecord r1 => r1.Address.ToString(),
                            AaaaRecord r2 => r2.Address.ToString(),
                            _ => null
                        };
                    })
                    .Where(t => t != null)
                    .ToHashSet();

                allIps.AddRange(mxIps);
            }

            Console.WriteLine("{1,-30}:\tGot {0} non unique A-/AAAA-records", allIps.Count, domain);
            return allIps.ToHashSet();
        }

        private static async Task<IDnsQueryResponse> DNSRequest(
            ILookupClient client,
            string domain,
            DnsClient.QueryType type
        )
        {
            IDnsQueryResponse lastError = null;

            for (var i = 0; i < 10; i++)
            {
                try
                {
                    var result = await client.QueryAsync(domain, type);
                    if (result.HasError)
                    {
                        Console.WriteLine("Can not get DNS records for {0}. Type {1} from {2}. Error: {3}",
                            domain,
                            type,
                            client.NameServers.First().Address,
                            result.ErrorMessage
                        );
                        lastError = result;
                        continue;
                    }

                    return result;
                }
                catch (DnsResponseException)
                {
                    Console.WriteLine("Can not get DNS records for {0}. Type {1} from {2}",
                        domain,
                        type,
                        client.NameServers.First().Address
                    );
                }
            }

            return lastError;
        }
    }
}