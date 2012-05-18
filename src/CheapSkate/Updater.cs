using System;
using System.Linq;

namespace CheapSkate
{
    public class Updater
    {
        private readonly ILogger _logger;
        private readonly ICache _cache;
        private readonly DynamicDns _dynamicDns;

        public Updater(ILogger logger, ICache cache, DynamicDns dynamicDns)
        {
            _logger = logger;
            _cache = cache;
            _dynamicDns = dynamicDns;
        }

        public bool Update(Options options)
        {
            try
            {
                var domain = options.SubDomain + "." + options.Domain;
                var result = _dynamicDns.Update(options.SubDomain, options.Domain, options.ApiKey, options.IpAddress);
                if (result.Errors.Any())
                {
                    _logger.Log("An error has occured pointing the DNS record {0} to {1}: {2}",
                        domain,
                        string.IsNullOrEmpty(options.IpAddress) ? "auto detected IP address" : options.IpAddress,
                        result.Errors.Aggregate((a, i) => a + ", " + i));
                    return false;
                }
                var cache = _cache.Load();
                if (cache.ContainsKey(domain) && cache[domain] != result.IpAddress)
                {
                    _logger.Log("IP address for {0} changed from {1} to {2}.", domain, cache[domain], result.IpAddress);
                    cache[domain] = result.IpAddress;
                }
                else if (!cache.ContainsKey(domain))
                {
                    _logger.Log("Set IP address for {0} to {1}.", domain, result.IpAddress);
                    cache.Add(domain, result.IpAddress);
                }
                _cache.Save(cache);
            }
            catch (Exception e)
            {
                _logger.Log("Doh, an application error has occured: \r\n\r\n{0}", e);
                return false;
            }
            return true;
        }
    }
}