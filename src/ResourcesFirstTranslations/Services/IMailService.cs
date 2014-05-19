using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ResourcesFirstTranslations.Services
{
    public class MailMessage
    {
        public MailMessage()
        {
        }

        public MailMessage(string from, string to, string subject, string body)
        {
            From = from;
            To = new List<string>() { to };
            Subject = subject;
            Body = body;
        }

        public string From { get; set; }
        public List<string> To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }

    public interface IMailService
    {
        Task<bool> SendMailAsync(MailMessage mm, bool suppressExceptions = true);
    }
}
