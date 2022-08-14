# https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/docker/building-net-docker-images?view=aspnetcore-6.0
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS resolver

WORKDIR /source

# copy csproj and restore as distinct layers
COPY ./Resolver/*.sln .
COPY ./Resolver/Resolver.csproj .
RUN ls -la
RUN dotnet restore

# copy everything else and build app
COPY ./Resolver/ .
RUN dotnet publish -c Release -o /app

WORKDIR /app
COPY domains.txt .
RUN dotnet ./Resolver.dll -i domains.txt -o mailertable-patch.txt

FROM ubuntu:20.04
LABEL maintainer="Nokita Kaze <admin@kanaria.ru>"

RUN apt-get update && \
    apt-get install --yes --no-install-recommends sendmail

# RUN apt-get install --yes --no-install-recommends net-tools mc telnet

COPY --from=resolver /app/mailertable-patch.txt ./mailertable-patch.txt

RUN cat mailertable-patch.txt >> '/etc/mail/mailertable' && \
    cat /etc/mail/sendmail.mc | sed "s/MAILER_DEFINITIONS/FEATURE(mailertable, \`hash -o \/etc\/mail\/mailertable')\nMAILER_DEFINITIONS/" > /etc/mail/sendmail.mc.1 && \
    mv /etc/mail/sendmail.mc.1 /etc/mail/sendmail.mc && \
    cat /etc/mail/sendmail.mc | sed "s/DAEMON_OPTIONS(\`Family=inet,  Name=MTA-v4, Port=smtp, Addr=127.0.0.1')dnl/DAEMON_OPTIONS(\`Family=inet,  Name=MTA-v4, Port=smtp, Addr=0.0.0.0')dnl/" > /etc/mail/sendmail.mc.1 && \
    mv /etc/mail/sendmail.mc.1 /etc/mail/sendmail.mc && \
    cat /etc/mail/local-host-names | sed 's/localhost/localhost\nsendmail.example.com/' > /etc/mail/local-host-names.1 && \
    mv /etc/mail/local-host-names.1 /etc/mail/local-host-names && \
    echo 'Connect:192.168	RELAY' >> /etc/mail/access && \
    echo 'Connect:172	RELAY' >> /etc/mail/access && \
    rm mailertable-patch.txt

RUN cat /etc/mail/mailertable

CMD exec yes 'y' | sendmailconfig && \
    exec /bin/bash -c "trap : TERM INT; sleep infinity & wait"
