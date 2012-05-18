using System;
using CheapSkate;
using NSubstitute;
using NUnit.Framework;
using Should;

namespace Tests
{
    [TestFixture]
    public class UpdaterTests
    {
        private static readonly Func<Options> CreateOptions = () => new Options
            {
                ApiKey = "234r2341234", 
                Domain = "dundermifflin.com", 
                SubDomain = "intranet", 
                IpAddress = "192.168.1.1",
                Email = "test@test.com", 
                SmtpServer = "smtp.dundermifflin.com"
            };

        [Test]
        public void should_alert_on_the_first_update()
        {
            var options = CreateOptions();
            var logger = new FakeLogger();
            var cache = new FakeCache();
            var updater = new Updater(logger, cache, new DynamicDns(new FakeWebRequest(ipAddress: options.IpAddress)));
            updater.Update(options).ShouldBeTrue();
            logger.Entries.Count.ShouldEqual(1);
            logger.Entries[0].ShouldContain("Set IP address");
            cache.Values.Values.Count.ShouldEqual(1);
            cache.Values["intranet.dundermifflin.com"].ShouldEqual("192.168.1.1");
        }

        [Test]
        public void should_not_alert_with_no_change()
        {
            var options = CreateOptions();
            var logger = new FakeLogger();
            var cache = new FakeCache();
            var updater = new Updater(logger, cache, new DynamicDns(new FakeWebRequest(ipAddress: options.IpAddress)));
            cache.Values.Add("intranet.dundermifflin.com", "192.168.1.1");
            updater.Update(options).ShouldBeTrue();
            logger.Entries.Count.ShouldEqual(0);
            cache.Values.Values.Count.ShouldEqual(1);
            cache.Values["intranet.dundermifflin.com"].ShouldEqual("192.168.1.1");
        }

        [Test]
        public void should_alert_with_change()
        {
            var options = CreateOptions();
            var logger = new FakeLogger();
            var cache = new FakeCache();
            var updater = new Updater(logger, cache, new DynamicDns(new FakeWebRequest(ipAddress: "192.168.1.2")));
            cache.Values.Add("intranet.dundermifflin.com", "192.168.1.1");
            updater.Update(options).ShouldBeTrue();
            logger.Entries.Count.ShouldEqual(1);
            logger.Entries[0].ShouldContain("IP address for");
            cache.Values.Values.Count.ShouldEqual(1);
            cache.Values["intranet.dundermifflin.com"].ShouldEqual("192.168.1.2");
        }

        [Test]
        public void should_alert_on_service_error()
        {
            var options = CreateOptions();
            var logger = new FakeLogger();
            var cache = new FakeCache();
            var updater = new Updater(logger, cache, new DynamicDns(new FakeWebRequest(error: true)));
            updater.Update(options).ShouldBeFalse();
            logger.Entries.Count.ShouldEqual(1);
            logger.Entries[0].ShouldContain("An error has occured");
            cache.Values.Values.Count.ShouldEqual(0);
        }

        [Test]
        public void should_alert_on_app_error()
        {
            var options = CreateOptions();
            var logger = new FakeLogger();
            var cache = new FakeCache();
            var updater = new Updater(logger, cache, new DynamicDns(new FakeWebRequest(blowUp: true)));
            updater.Update(options).ShouldBeFalse();
            logger.Entries.Count.ShouldEqual(1);
            logger.Entries[0].ShouldContain("Doh, an application error");
            cache.Values.Values.Count.ShouldEqual(0);
        }
    }
}