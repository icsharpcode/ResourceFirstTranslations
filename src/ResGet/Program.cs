using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ResGet.Models;

namespace ResGet
{
    class Program
    {
        static int Main(string[] args)
        {
            var options = new Options();

            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                if (String.IsNullOrWhiteSpace(options.TargetDirectory))
                {
                    options.TargetDirectory = ".\\";
                }

                bool result = MainAsync(options).GetAwaiter().GetResult();

                return result ? 0 : 1;
            }

            return 2;
        }

        static async Task<bool> MainAsync(Options options)
        {
            var client = new RftResourcesClient(options.Url);
            bool result = false;

            // Fetch the resource file ids - we need them in any case
            List<int> fileIds = await client.GetResourceFileIdsForBranchAsync(options.Branch);

            if (!fileIds.Any())
            {
                Trace.TraceWarning("No resource files downloadable for branch, terminating");
                return false;
            }

            // Single language vs all of the files from the server
            if (!String.IsNullOrWhiteSpace(options.Language))
            {
                result = await DownloadAndSaveResourceFilesAsync(client, options.Branch, fileIds, options.Language,
                    options.Overwrite, options.TargetDirectory, options.Format);

                return result;
            }

            // Now we do multi-language
            var stats = await client.GetCulturesForBranchAsync(options.Branch);

            if (!stats.Any())
            {
                Trace.TraceWarning("No culture information retrieved from server, terminating");
                return false;
            }

            foreach (var culture in stats)
            {
                result = await DownloadAndSaveResourceFilesAsync(client, options.Branch, fileIds, culture.Culture,
                    options.Overwrite, options.TargetDirectory, options.Format);

                if (!result) return false;
            }

            return true;
        }

        static async Task<bool> DownloadAndSaveResourceFilesAsync(RftResourcesClient client, int branch,
            List<int> fileIds, string culture, bool overwriteExisting, string path, string format)
        {
            foreach (int fileId in fileIds)
            {
                Trace.TraceInformation("Downloading resource file {0} for language {1}, branch {2}", fileId, culture, branch);
                ResourceFile theFile = await client.GetResourceFileAsync(branch, fileId, culture, format);

                if (null != theFile)
                {
                    try
                    {
                        string filePath = Path.Combine(path, theFile.Filename);

                        if (File.Exists(filePath) && !overwriteExisting)
                        {
                            Trace.TraceError("File exists, overwriting was not enabled, aborting subsequent requests");
                            return false;
                        }

                        File.WriteAllBytes(filePath, theFile.Content);
                    }
                    catch (Exception ex)
                    {
                        Trace.TraceError(ex.ToString());
                        Trace.TraceError("Failed to save the file, aborting subsequent requests");
                        return false;
                    }
                }
                else
                {
                    Trace.TraceError("Failed to download the file, aborting subsequent requests");
                    return false;
                }
            }

            return true;
        }
    }
}
