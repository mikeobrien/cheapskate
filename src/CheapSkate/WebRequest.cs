using System;
using System.Linq;
using System.Net;
using System.Web;

namespace CheapSkate
{
    public interface IWebRequest
    {
        string Get(string url, params object[] args);
    }

    public class WebRequest : IWebRequest
    {
        public string Get(string url, params object[] args)
        {
            return new WebClient().DownloadString(
                new Uri(string.Format(url, args.Select(x => HttpUtility.UrlEncode(x.ToString())).ToArray())));
        } 
    }
}