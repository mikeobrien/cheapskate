using System.Collections.Generic;
using CheapSkate;

namespace Tests
{
    public class FakeLogger : ILogger
    {
        public FakeLogger()
        {
            Entries = new List<string>();
        }

        public List<string> Entries { get; set; }

        public void Log(string message, params object[] args)
        {
            Entries.Add(string.Format(message, args));
        }
    }
}