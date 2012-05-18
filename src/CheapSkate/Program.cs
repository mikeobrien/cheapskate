using System;
using System.Linq;
using System.Reflection;

namespace CheapSkate
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine();
            Console.WriteLine("CheapSkate {0} - NameCheap DDNS Client", Assembly.GetExecutingAssembly().GetName().Version);
            Console.WriteLine();

            if (args.Length == 0)
            {
                var descriptions = OptionParser.GetOptions<Options>();
                descriptions.ToList().ForEach(x => Console.WriteLine("    -{0}{1}", x.Key.PadRight(10), x.Value));
                return;
            }

            Options options = null;
            try
            {
                options = OptionParser.Parse<Options>(args);
            }
            catch (MissingArgumentsException e)
            {
                Console.WriteLine(e.Message);
                Environment.Exit(1);
            }

            var filename = Assembly.GetEntryAssembly().GetName().Name;
            var consoleLogger = new ConsoleLogger();
            var fileLogger = new FileLogger(filename + ".log");
            var logger = string.IsNullOrEmpty(options.Email) ? new CompositeLogger(fileLogger, consoleLogger) : 
                new CompositeLogger(fileLogger, consoleLogger,
                    new EmailLogger(options.SmtpServer, 
                                    "CheapSkate Alert",
                                    string.Format("cheapskate@{0}", options.Domain),
                                    options.Email));
            var cache = new Cache(filename + ".cache");
            var dynamicDns = new DynamicDns(new WebRequest());

            Console.WriteLine("Connecting...");

            if (!new Updater(logger, cache, dynamicDns).Update(options))
            {
                Console.WriteLine("Operation completed with errors. :(");
                Environment.Exit(1);
            }
            Console.WriteLine("Operation completed successfully. :D");
        }
    }
}
