using System.Collections.Generic;
using System.IO;
using CheapSkate;
using NUnit.Framework;
using Should;

namespace Tests
{
    [TestFixture]
    public class CacheTests
    {
        [Test]
        public void should_read_cache()
        {
            var path = Path.GetTempFileName();
            File.WriteAllLines(path, new[] { "one=1", "2=two" });
            var cache = new Cache(path).Load();
            cache["one"].ShouldEqual("1");
            cache["2"].ShouldEqual("two");
            File.Delete(path);
        }

        [Test]
        public void should_missing_read_cache()
        {
            var cache = new Cache(Path.Combine(Path.GetTempPath(), "yada.cache")).Load();
            cache.Count.ShouldEqual(0);
        }

        [Test]
        public void should_save_cache()
        {
            var path = Path.GetTempFileName();
            var cache = new Dictionary<string, string> {{"one", "1"}, {"2", "two"}};
            new Cache(path).Save(cache);
            var file = File.ReadAllLines(path);
            file[0].ShouldEqual("one=1");
            file[1].ShouldEqual("2=two");
            File.Delete(path);
        }
    }
}