using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
            var lines = new List<string>();
            foreach (var domain in domains)
            {
                var ips = await GetDomain(domain);
                if (!ips.Any())
                {
                    Console.WriteLine("Domain {0} contains no MX-elements", domain);
                    continue;
                }

                var smtpLine = string.Concat(ips.Select(ip => string.Format(":[{0}]", ip)));
                lines.Add(string.Format(
                    "    echo '{0} smtp{1}' >> '/etc/mail/mailertable' && \\",
                    domain,
                    smtpLine
                ));
            }

            File.WriteAllLines(args[1], lines);
        }

        private static async Task<ICollection<string>> GetDomain(string domain)
        {
            var lookup = new LookupClient();
            var result = await lookup.QueryAsync(domain, QueryType.MX);

            Console.WriteLine("\nGet MX-records for {0}", domain);
            var mxDomains = result
                .Answers
                .Select(t => t as MxRecord)
                .OrderBy(t => t!.Preference)
                .Select(t => t.Exchange.Value)
                .ToArray();

            Console.WriteLine("Got {0} MX-records", mxDomains.Length);
            var allIps = new List<string>();
            foreach (var mxDomain in mxDomains)
            {
                result = await lookup.QueryAsync(mxDomain, QueryType.ANY);
                var mxIps = result
                    .Answers
                    .Where(t => (t.RecordType == ResourceRecordType.AAAA) ||
                                (t.RecordType == ResourceRecordType.A))
                    .Select(t =>
                    {
                        return t switch
                        {
                            ARecord r1 => r1.Address.ToString(),
                            AaaaRecord r2 => r2.Address.ToString(),
                            _ => null
                        };
                    })
                    .ToHashSet();

                if (mxIps.Any())
                {
                    allIps.AddRange(mxIps);
                    continue;
                }

                var soaRaw = result
                    .Answers
                    .FirstOrDefault(t => t.RecordType == ResourceRecordType.SOA);
                if (!(soaRaw is SoaRecord soa))
                {
                    continue;
                }

                var soaResult = await lookup.QueryAsync(soa.MName, QueryType.ANY);
                var soaARecord = soaResult
                    .Answers
                    .Where(t => (t.RecordType == ResourceRecordType.AAAA) ||
                                (t.RecordType == ResourceRecordType.A))
                    .Select(t =>
                    {
                        return t switch
                        {
                            ARecord r1 => r1.Address.ToString(),
                            AaaaRecord r2 => r2.Address.ToString(),
                            _ => null
                        };
                    })
                    .FirstOrDefault();
                if (soaARecord == null)
                {
                    continue;
                }

                var lookupLocal = new LookupClient(IPAddress.Parse(soaARecord));
                result = await lookupLocal.QueryAsync(mxDomain, QueryType.ANY);
                mxIps = result
                    .Answers
                    .Where(t => (t.RecordType == ResourceRecordType.AAAA) ||
                                (t.RecordType == ResourceRecordType.A))
                    .Select(t =>
                    {
                        return t switch
                        {
                            ARecord r1 => r1.Address.ToString(),
                            AaaaRecord r2 => r2.Address.ToString(),
                            _ => null
                        };
                    })
                    .ToHashSet();

                allIps.AddRange(mxIps);
            }

            Console.WriteLine("Got {0} non unique A-/AAAA-records", allIps.Count);
            return allIps.ToHashSet();
        }
    }
}