CheapSkate
=============

CheapSkate is a CLI for [NameCheap](http://www.namecheap.com/) DDNS.
	
## Command line reference

    cheapskate.exe [options]
    
    -sd      subdomain to update [required]
    -d       domain name [required]
    -ip      ip address to set, ommit for auto detect [optional]
    -key     api key [required]
    -email   address to send email alerts to. email alerts are only
             sent when the ip address changes or an error occurs [optional]
    -smtp    smtp server for sending email alerts [optional]