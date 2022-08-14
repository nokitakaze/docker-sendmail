# Sendmail docker
[![Docker pulls](https://badgen.net/docker/pulls/nokitakaze/sendmail)](https://hub.docker.com/r/nokitakaze/sendmail)
[![Docker stars](https://badgen.net/docker/stars/nokitakaze/sendmail?icon=docker&label=stars)](https://hub.docker.com/r/nokitakaze/sendmail)
[![Docker image size](https://badgen.net/docker/size/nokitakaze/sendmail)](https://hub.docker.com/r/nokitakaze/sendmail)

Just a simple configured sendmail for sending mails.

## Start the server

Your `docker-compose.yml`:
```yml
version: "3"
services:
   sendmail_itself:
      container_name: ubuntu_sendmail
      image: nokitakaze/sendmail
      ports:
         - "127.0.0.1:2025:25"
      hostname: sendmail.example.com
      volumes:
         - "./sendmaildb:/var/run/sendmail"
         - "./mails:/var/spool/mqueue"
```

Then:
```sh
docker-compose up -d
```
