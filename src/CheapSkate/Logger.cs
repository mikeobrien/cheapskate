using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;

namespace CheapSkate
{
    public interface ILogger
    {
        void Log(string message, params object[] args);
    }

    public class CompositeLogger : ILogger
    {
        private readonly List<ILogger> _loggers;

        public CompositeLogger(params ILogger[] loggers)
        {
            _loggers = loggers.ToList();
        }

        public void Log(string message, params object[] args)
        {
            _loggers.ForEach(x => x.Log(message, args));
        }
    }

    public class ConsoleLogger : ILogger
    {
        public void Log(string message, params object[] args)
        {
            Console.WriteLine(message, args);
        }
    }

    public class FileLogger : ILogger
    {
        private readonly string _path;

        public FileLogger(string path)
        {
            _path = path;
        }

        public void Log(string message, params object[] args)
        {
            message = string.Format(message, args);
            File.AppendAllText(_path, string.Format("{0}: {1}", DateTime.Now, message));
        }
    }

    public class EmailLogger : ILogger
    {
        private readonly string _smtpServer;
        private readonly string _subject;
        private readonly string _from;
        private readonly string _to;

        public EmailLogger(string smtpServer, string subject, string from, string to)
        {
            _smtpServer = smtpServer;
            _subject = subject;
            _from = @from;
            _to = to;
        }

        public void Log(string message, params object[] args)
        {
            message = string.Format(message, args);
            var mailMessage = new MailMessage(_from, _to);
            mailMessage.Subject = _subject;
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = message.Replace("\r\n", "<br/>");
            (string.IsNullOrEmpty(_smtpServer) ? new SmtpClient() : new SmtpClient(_smtpServer)).Send(mailMessage);
        }
    }
}