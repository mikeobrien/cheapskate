using System.IO;
using CheapSkate;
using NUnit.Framework;
using Should;

namespace Tests
{
    [TestFixture]
    public class LoggerTests
    {
        [Test]
        public void should_log_to_a_file()
        {
            var path = Path.GetTempFileName();
            var logger = new FileLogger(path);
            logger.Log("oh hai {0}", "yo");
            var log = File.ReadAllLines(path);
            log.Length.ShouldEqual(1);
            log[0].EndsWith(": oh hai yo").ShouldBeTrue();
            File.Delete(path);
        }

        [Test]
        public void should_log_to_email()
        {
            new EmailLogger("smtp-server.roadrunner.com", "CheapSkate Integration Test Email", 
                "cheapskate@mikeobrien.net", "mob@mikeobrien.net").Log("oh hai {0}", "yo");
        }
    }
}