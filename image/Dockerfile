FROM ubuntu:20.04

LABEL maintainer="Nokita Kaze <admin@kanaria.ru>"

RUN apt-get update && \
    apt-get install --yes --no-install-recommends sendmail

# RUN apt-get install --yes --no-install-recommends net-tools mc telnet

RUN echo 'gmail.com smtp:[2a00:1450:4010:c0a::1b]:[108.177.97.26]:[108.177.97.27]:[2404:6800:4008:c00::1b]:[2404:6800:4008:c00::1a]:[74.125.28.26]:[74.125.28.27]:[2607:f8b0:400e:c04::1b]:[2607:f8b0:400e:c04::1a]:[173.194.223.27]:[2607:f8b0:4003:c10::1b]:[108.177.112.27]' >> '/etc/mail/mailertable' && \
    echo 'yandex.ru smtp:[2a02:6b8::89]:[213.180.193.89]:[87.250.250.89]:[77.88.21.89]:[93.158.134.89]:[213.180.204.89]' >> '/etc/mail/mailertable' && \
    echo 'yandex.com smtp:[213.180.193.89]:[87.250.250.89]:[77.88.21.89]:[93.158.134.89]:[213.180.204.89]:[2a02:6b8::89]' >> '/etc/mail/mailertable' && \
    echo 'rambler.ru smtp:[81.19.78.67]:[81.19.78.65]:[81.19.78.66]:[81.19.78.64]' >> '/etc/mail/mailertable' && \
    echo 'mail.ru smtp:[94.100.180.104]:[94.100.180.31]' >> '/etc/mail/mailertable' && \
    echo 'bk.ru smtp:[94.100.180.104]:[94.100.180.31]' >> '/etc/mail/mailertable' && \
    echo 'list.ru smtp:[94.100.180.104]:[94.100.180.31]' >> '/etc/mail/mailertable' && \
    echo 'mail.ua smtp:[94.100.180.104]:[94.100.180.31]' >> '/etc/mail/mailertable' && \
    echo 'yahoo.com smtp:[67.195.204.79]:[67.195.228.94]:[67.195.228.110]:[67.195.228.109]:[67.195.204.73]:[98.136.96.91]:[67.195.204.77]:[98.136.96.74]:[67.195.204.72]:[98.136.96.75]:[67.195.204.74]:[98.136.96.76]' >> '/etc/mail/mailertable' && \
    echo 'hotmail.com smtp:[104.47.18.97]:[104.47.17.161]' >> '/etc/mail/mailertable' && \
    echo 'outlook.com smtp:[104.47.70.33]:[104.47.58.33]' >> '/etc/mail/mailertable' && \
    echo 'protonmail.com smtp:[185.70.40.103]:[185.70.40.102]' >> '/etc/mail/mailertable' && \
    echo 'kleogb.com smtp:[45.55.104.203]' >> '/etc/mail/mailertable' && \
    echo 'cock.li smtp:[2001:ac8:7d:1e::c0cc:4]:[37.120.193.125]:[2c0f:f930:0:5::92]:[198.167.223.92]' >> '/etc/mail/mailertable' && \
    echo 'airmail.cc smtp:[2001:ac8:7d:1e::c0cc:4]:[37.120.193.125]:[2c0f:f930:0:5::92]:[198.167.223.92]' >> '/etc/mail/mailertable' && \
    echo '420blaze.it smtp:[2001:ac8:7d:1e::c0cc:4]:[37.120.193.125]:[2c0f:f930:0:5::92]:[198.167.223.92]' >> '/etc/mail/mailertable' && \
    echo 'aaathats3as.com smtp:[2001:ac8:7d:1e::c0cc:4]:[37.120.193.125]:[2c0f:f930:0:5::92]:[198.167.223.92]' >> '/etc/mail/mailertable' && \
    echo 'cumallover.me smtp:[2001:ac8:7d:1e::c0cc:4]:[37.120.193.125]:[2c0f:f930:0:5::92]:[198.167.223.92]' >> '/etc/mail/mailertable' && \
    echo 'goat.si smtp:[2001:ac8:7d:1e::c0cc:4]:[37.120.193.125]:[2c0f:f930:0:5::92]:[198.167.223.92]' >> '/etc/mail/mailertable' && \
    echo 'horsefucker.org smtp:[2001:ac8:7d:1e::c0cc:4]:[37.120.193.125]:[2c0f:f930:0:5::92]:[198.167.223.92]' >> '/etc/mail/mailertable' && \
    echo 'national.shitposting.agency smtp:[2001:ac8:7d:1e::c0cc:4]:[37.120.193.125]:[2c0f:f930:0:5::92]:[198.167.223.92]' >> '/etc/mail/mailertable' && \
    echo 'nigge.rs smtp:[2001:ac8:7d:1e::c0cc:4]:[37.120.193.125]:[198.167.223.92]:[2c0f:f930:0:5::92]' >> '/etc/mail/mailertable' && \
    echo 'tfwno.gf smtp:[2001:ac8:7d:1e::c0cc:4]:[37.120.193.125]:[2c0f:f930:0:5::92]:[198.167.223.92]' >> '/etc/mail/mailertable' && \
    echo 'cock.lu smtp:[2001:ac8:7d:1e::c0cc:4]:[37.120.193.125]:[2c0f:f930:0:5::92]:[198.167.223.92]' >> '/etc/mail/mailertable' && \
    echo 'cock.email smtp:[2001:ac8:7d:1e::c0cc:4]:[37.120.193.125]:[198.167.223.92]:[2c0f:f930:0:5::92]' >> '/etc/mail/mailertable' && \
    echo 'firemail.cc smtp:[2001:ac8:7d:1e::c0cc:4]:[37.120.193.125]:[2c0f:f930:0:5::92]:[198.167.223.92]' >> '/etc/mail/mailertable' && \
    echo 'hitler.rocks smtp:[2001:ac8:7d:1e::c0cc:4]:[37.120.193.125]:[2c0f:f930:0:5::92]:[198.167.223.92]' >> '/etc/mail/mailertable' && \
    echo 'getbackinthe.kitchen smtp:[2001:ac8:7d:1e::c0cc:4]:[37.120.193.125]:[2c0f:f930:0:5::92]:[198.167.223.92]' >> '/etc/mail/mailertable' && \
    echo 'memeware.net smtp:[2001:ac8:7d:1e::c0cc:4]:[37.120.193.125]:[2c0f:f930:0:5::92]:[198.167.223.92]' >> '/etc/mail/mailertable' && \
    echo 'cocaine.ninja smtp:[2001:ac8:7d:1e::c0cc:4]:[37.120.193.125]:[2c0f:f930:0:5::92]:[198.167.223.92]' >> '/etc/mail/mailertable' && \
    echo 'waifu.club smtp:[2001:ac8:7d:1e::c0cc:4]:[37.120.193.125]:[2c0f:f930:0:5::92]:[198.167.223.92]' >> '/etc/mail/mailertable' && \
    echo 'rape.lol smtp:[2001:ac8:7d:1e::c0cc:4]:[37.120.193.125]:[2c0f:f930:0:5::92]:[198.167.223.92]' >> '/etc/mail/mailertable' && \
    echo 'nuke.africa smtp:[2001:ac8:7d:1e::c0cc:4]:[37.120.193.125]:[198.167.223.92]:[2c0f:f930:0:5::92]' >> '/etc/mail/mailertable' && \
    cat /etc/mail/sendmail.mc | sed "s/MAILER_DEFINITIONS/FEATURE(mailertable, \`hash -o \/etc\/mail\/mailertable')\nMAILER_DEFINITIONS/" > /etc/mail/sendmail.mc.1 && \
    mv /etc/mail/sendmail.mc.1 /etc/mail/sendmail.mc && \
    cat /etc/mail/sendmail.mc | sed "s/DAEMON_OPTIONS(\`Family=inet,  Name=MTA-v4, Port=smtp, Addr=127.0.0.1')dnl/DAEMON_OPTIONS(\`Family=inet,  Name=MTA-v4, Port=smtp, Addr=0.0.0.0')dnl/" > /etc/mail/sendmail.mc.1 && \
    mv /etc/mail/sendmail.mc.1 /etc/mail/sendmail.mc && \
    cat /etc/mail/local-host-names | sed 's/localhost/localhost\nsendmail.example.com/' > /etc/mail/local-host-names.1 && \
    mv /etc/mail/local-host-names.1 /etc/mail/local-host-names && \
    echo 'Connect:192.168	RELAY' >> /etc/mail/access && \
    echo 'Connect:172	RELAY' >> /etc/mail/access

CMD exec yes 'y' | sendmailconfig && \
    exec /bin/bash -c "trap : TERM INT; sleep infinity & wait"
