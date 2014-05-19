using System;
using System.Net;
using System.Net.Mail;
using System.Diagnostics;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SendGridMail;

namespace ResourcesFirstTranslations.Services
{
    // http://sendgrid.com/docs/Code_Examples/csharp.html
    public class SendGridMailService : IMailService
    {
        private readonly IConfigurationService _configurationService;

        public SendGridMailService(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        public async Task<bool> SendMailAsync(MailMessage mm, bool suppressExceptions=true)
        {
            try
            {
                string userName = _configurationService.SendGridUsername;
                string password = _configurationService.SendGridPassword;

                SendGrid message = SendGrid.GetInstance();

                message.From = new MailAddress(mm.From);
                message.AddTo(mm.To);
                message.Subject = mm.Subject;
                message.Text = mm.Body;

                var credentials = new NetworkCredential(userName, password);
                var transportWeb = Web.GetInstance(credentials);
                await transportWeb.DeliverAsync(message).ConfigureAwait(false);

                return true;
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());

                if (!suppressExceptions)
                    throw;
            }

            return false;
        }
    }
}