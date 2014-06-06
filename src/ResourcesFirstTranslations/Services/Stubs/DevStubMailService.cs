using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourcesFirstTranslations.Services.Stubs
{
    public class DevStubMailService : IMailService
    {
        public Task<bool> SendMailAsync(MailMessage mm, bool suppressExceptions = true)
        {
            Debug.WriteLine("--- DevStub sending email ---");
            Debug.WriteLine(mm.Subject);
            Debug.WriteLine(mm.Body);

            return Task.FromResult(true);
        }
    }
}
