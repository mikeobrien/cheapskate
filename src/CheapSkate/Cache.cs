using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CheapSkate
{
    public interface ICache
    {
        Dictionary<string, string> Load();
        void Save(Dictionary<string, string> cache);
    }

    public class Cache : ICache
    {
        private readonly string _path;

        public Cache(string path)
        {
            _path = path;
        }

        public Dictionary<string, string> Load()
        {
            if (!File.Exists(_path)) return new Dictionary<string, string>();
            return File.ReadAllLines(_path).Select(x => x.Split('=')).ToDictionary(x => x[0], x => x[1]);
        }

        public void Save(Dictionary<string, string> cache)
        {
            File.WriteAllLines(_path, cache.Select(x => x.Key + "=" + x.Value));
        }
    }
}