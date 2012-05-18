namespace CheapSkate
{
    public class Options
    {
        public Options()
        {
            IpAddress = "";
        }

        [Option("sd", "Subdomain to update", true)]
        public string SubDomain { get; set; }
        [Option("d", "Domain name", true)]
        public string Domain { get; set; }
        [Option("ip", "IP address to set, blank for auto detect", false)]
        public string IpAddress { get; set; }
        [Option("key", "API key", true)]
        public string ApiKey { get; set; }
        [Option("smtp", "SMTP server for sending email", false)]
        public string SmtpServer { get; set; }
        [Option("email", "Recipient of email alerts", false)]
        public string Email { get; set; }
    }
}