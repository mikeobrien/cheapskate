using System.Diagnostics;
using System.Linq;
using CheapSkate;
using NUnit.Framework;
using Should;

namespace Tests
{
    [TestFixture]
    public class DynamicDnsTests
    {
        [Test]
        public void should_parse_success_response()
        {
            var result = new DynamicDns(new FakeWebRequest(false)).Update("labs", "dundermifflin.com", "21k34h21g34g123h1g23134k43h1k23g4", "207.97.227.245");
            result.IpAddress.ShouldEqual("207.97.227.245");
            result.Errors.ShouldBeEmpty();
        }

        [Test]
        public void should_parse_fail_response()
        {
            var result = new DynamicDns(new FakeWebRequest(true)).Update("labs", "dundermifflin.com", "21k34h21g34g123h1g23134k43h1k23g4", "207.97.227.245");
            result.IpAddress.ShouldBeEmpty();
            result.Errors.Count().ShouldEqual(2);
            result.Errors.First().ShouldEqual("Domain name not found");
            result.Errors.Skip(1).First().ShouldEqual("Invalid key");
        }
    }
}