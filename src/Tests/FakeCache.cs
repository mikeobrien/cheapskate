using System.Collections.Generic;
using CheapSkate;

namespace Tests
{
    public class FakeCache : ICache
    {
        public FakeCache()
        {
            Values = new Dictionary<string, string>();
        }

        public Dictionary<string, string> Values { get; set; }
 
        public Dictionary<string, string> Load()
        {
            return Values;
        }

        public void Save(Dictionary<string, string> cache)
        {
            Values = cache;
        }
    }
}