FROM ubuntu:20.04

LABEL maintainer="Nokita Kaze <admin@kanaria.ru>"

RUN apt-get update && \
    apt-get install --yes --no-install-recommends sendmail

# RUN apt-get install --yes --no-install-recommends net-tools mc telnet

RUN echo 'gmail.com smtp:[64.233.163.27]:[108.177.97.27]:[74.125.28.26]:[173.194.70.27]:[209.85.146.26]' >> '/etc/mail/mailertable' && \
    echo 'yandex.ru smtp:[77.88.21.89]:[93.158.134.89]:[213.180.193.89]:[213.180.204.89]:[87.250.250.89]' >> '/etc/mail/mailertable' && \
    echo 'mail.ru smtp:[94.100.180.31]:[94.100.180.104]' >> '/etc/mail/mailertable' && \
    echo 'hotmail.com smtp:[104.47.125.33]:[104.47.124.33]' >> '/etc/mail/mailertable' && \
    echo 'yahoo.com smtp:[67.195.204.79]:[67.195.204.74]:[67.195.228.106]:[67.195.228.94]:[98.136.96.74]:[98.136.96.76]:[98.136.96.75]:[67.195.204.72]:[98.136.96.76]:[67.195.228.94]:[67.195.204.79]:[67.195.228.110]:[98.136.96.77]:[67.195.204.73]:[98.136.96.91]' >> '/etc/mail/mailertable' && \
    echo 'outlook.com smtp:[104.47.57.161]:[104.47.56.161]' >> '/etc/mail/mailertable' && \
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
