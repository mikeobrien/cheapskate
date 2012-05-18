using System;
using CheapSkate;
using NUnit.Framework;
using Should;

namespace Tests
{
    [TestFixture]
    public class OptionsParserTests
    {
        [Test]
        public void should_parse_options()
        {
            var args = new[] { "-sd", "intranet", "-d", "dundermiffliin.com", "-key", "34234r3234t34twqret234tr1234",
                               "-smtp", "smtp.dundermifflin.com", "-email", "admin@dundermifflin.com" };
            var options = OptionParser.Parse<Options>(args);
            options.ApiKey.ShouldEqual("34234r3234t34twqret234tr1234");
            options.Domain.ShouldEqual("dundermiffliin.com");
            options.IpAddress.ShouldEqual("");
            options.SmtpServer.ShouldEqual("smtp.dundermifflin.com");
            options.SubDomain.ShouldEqual("intranet");
            options.Email.ShouldEqual("admin@dundermifflin.com");
        }

        [Test]
        public void should_throw_exception_when_required_argument_is_not_specified()
        {
            var args = new[] { "-key", "34234r3234t34twqret234tr1234", "-smtp", "smtp.dundermifflin.com", 
                               "-email", "admin@dundermifflin.com" };
            Assert.Throws<MissingArgumentsException>(() => OptionParser.Parse<Options>(args));
        }

        [Test]
        public void should_return_descriptions()
        {
            var descriptions = OptionParser.GetOptions<Options>();
            descriptions["sd"].ShouldNotBeEmpty();
            descriptions["d"].ShouldNotBeEmpty();
            descriptions["ip"].ShouldNotBeEmpty();
            descriptions["key"].ShouldNotBeEmpty();
            descriptions["smtp"].ShouldNotBeEmpty();
            descriptions["email"].ShouldNotBeEmpty();
        }
    }
}