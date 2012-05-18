using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Xml.Linq;

namespace CheapSkate
{
    public class NotConnectedException : Exception 
    { public NotConnectedException() : base("Unable to find a network connection.") {}}

    public class DynamicDns
    {
        private readonly IWebRequest _webRequest;

        public DynamicDns(IWebRequest webRequest)
        {
            _webRequest = webRequest;
        }

        public UpdateStatus Update(string subDomain, string domain, string apiKey, string ipAddress = "")
        {
            if (!NetworkInterface.GetIsNetworkAvailable()) throw new NotConnectedException();
            
            const string url = "https://dynamicdns.park-your-domain.com/update?by=nc&host={0}&domain={1}&password={2}&ip={3}";
            var result = _webRequest.Get(url, subDomain, domain, apiKey, ipAddress);
            var document = XDocument.Parse(result).Element("interface-response");
            var hasErrors = document.Element("ErrCount").Value != "0";
            return hasErrors ? 
                new UpdateStatus(document.Element("errors").Elements().Select(x => x.Value)) : 
                new UpdateStatus(document.Element("IP").Value);
        }

        public class UpdateStatus
        {
            public UpdateStatus(string ipAddress)
            {
                Errors = Enumerable.Empty<string>();
                IpAddress = ipAddress;
            }

            public UpdateStatus(IEnumerable<string> errors)
            {
                Errors = errors;
                IpAddress = "";
            }

            public string IpAddress { get; private set; }
            public IEnumerable<string> Errors { get; private set; }
        }
    }
}