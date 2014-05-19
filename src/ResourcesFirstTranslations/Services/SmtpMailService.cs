using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ResourcesFirstTranslations.Services
{
    public class SmtpMailService : IMailService
    {
        private readonly IConfigurationService _configurationService;

        public SmtpMailService(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        public Task<bool> SendMailAsync(MailMessage mm, bool suppressExceptions = true)
        {
            try
            {
                string userName = _configurationService.SmtpUsername;
                string password = _configurationService.SmtpPassword;
                string host = _configurationService.SmtpHost;
                string port = _configurationService.SmtpPort;
                int portNumeric = 25;
                Int32.TryParse(port, out portNumeric);

                var client = new SmtpClient()
                {
                    Credentials = new NetworkCredential(userName, password),
                    Host = host,
                    Port = portNumeric
                };

                var msg = new System.Net.Mail.MailMessage();
                msg.From = new MailAddress(mm.From);
                msg.Body = mm.Body;
                msg.Subject = mm.Subject;
                foreach (var to in mm.To)
                {
                    msg.To.Add(to);
                }
                
                client.Send(msg);   // SendAsync isn't really Task<T>
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());

                if (!suppressExceptions)
                    throw;
            }

            return Task.FromResult(false);
        }
    }
}
