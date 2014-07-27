using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ResGet.Models;

namespace ResGet
{
    class RftResourcesClient
    {
        private readonly string _baseUrl;

        public RftResourcesClient(string baseUrl)
        {
            _baseUrl = baseUrl;
        }

        public async Task<List<int>> GetResourceFileIdsForBranchAsync(int branch)
        {
            var url = _baseUrl + "ResourceFiles?branch=" + branch;
            var data = await GetAsStringAsync(url).ConfigureAwait(false);

            try
            {
                var ids = JsonConvert.DeserializeObject<List<int>>(data);
                return ids;
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
            }

            return new List<int>();
        }

        public async Task<List<CultureStatistics>> GetCulturesForBranchAsync(int branch)
        {
            var url = _baseUrl + "Missing?branch=" + branch;
            var data = await GetAsStringAsync(url).ConfigureAwait(false);

            try
            {
                var stats = JsonConvert.DeserializeObject<List<CultureStatistics>>(data);
                return stats;
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
            }

            return new List<CultureStatistics>();
        }

        public async Task<ResourceFile> GetResourceFileAsync(int branch, int file, string culture, string format)
        {
            var url = String.Format("{0}For?branch={1}&file={2}&culture={3}&format={4}", _baseUrl, branch, file, culture, format);

            var client = CreateHttpClient();
            try
            {
                using (var response = await client.GetAsync(url).ConfigureAwait(false))
                {
                    response.EnsureSuccessStatusCode();

                    byte[] body = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
                    string filename = response.Content.Headers.ContentDisposition.FileName;
                    
                    return new ResourceFile()
                    {
                        Filename = filename,
                        Content = body
                    };
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
            }

            return null;
        }

        private HttpClient CreateHttpClient()
        {
            var handler = new HttpClientHandler();

            if (handler.SupportsAutomaticDecompression)
            {
                handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            }

            var client = new HttpClient(handler);
            return client;
        }

        private async Task<string> GetAsStringAsync(string url)
        {
            var client = CreateHttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

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
    }
}
