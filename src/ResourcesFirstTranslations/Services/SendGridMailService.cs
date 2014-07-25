using System;
using System.Net;
using System.Net.Mail;
using System.Diagnostics;
using System.Threading.Tasks;
using SendGrid;

namespace ResourcesFirstTranslations.Services
{
    // https://github.com/sendgrid/sendgrid-csharp#how-to-create-an-email
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

                var message = new SendGridMessage();

                message.From = new MailAddress(mm.From);
                message.AddTo(mm.To);
                message.Subject = mm.Subject;
                message.Text = mm.Body;

                var credentials = new NetworkCredential(userName, password);
                var transportWeb = new Web(credentials);
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