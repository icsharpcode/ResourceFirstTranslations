using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ResourcesFirstTranslations.Services
{
    public class DefaultResxLoader : IResxLoader
    {
        public async Task<string> GetAsStringAsync(string url)
        {
            var handler = new HttpClientHandler();

            if (handler.SupportsAutomaticDecompression)
            {
                handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            }

            var client = new HttpClient(handler);

            try
            {
                using (var response = await client.GetAsync(url).ConfigureAwait(false))
                {
                    response.EnsureSuccessStatusCode();

                    var body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return body;
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
            }

            return null;
        }

        public string GetAsString(string url, bool suppressExceptions = true)
        {
            string responseString = null;

            try
            {
                WebRequest request = WebRequest.Create(url);
                using (WebResponse response = request.GetResponse())
                {
                    Stream dataStream = response.GetResponseStream();
                    using (var reader = new StreamReader(dataStream))
                    {
                        responseString = reader.ReadToEnd();
                        reader.Close();
                    }

                    response.Close();
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());

                if (!suppressExceptions)
                    throw;
            }

            return responseString;
        }
    }
}
