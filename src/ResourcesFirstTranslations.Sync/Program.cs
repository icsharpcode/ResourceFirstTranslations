using System;
using System.Diagnostics;
using Microsoft.WindowsAzure.Jobs;
using ResourcesFirstTranslations.Services;
using ResourcesFirstTranslations.Sync;

namespace ResourcesFirstTranslations.SyncJob
{
    class Program
    {
        static int Main(string[] args)
        {
            var options = new Options();

            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                if (0 == String.Compare(options.JobType, "azure", StringComparison.CurrentCultureIgnoreCase))
                {
                    Debug.WriteLine("Running in Azure WebJob mode");
                    var host = new JobHost();
                    host.Call(typeof(Program).GetMethod("SyncBranchResourceFiles"));
                }
                else
                {
                    Debug.WriteLine("Running in local mode");
                    SyncBranchResourceFiles();
                }

                return 0;
            }

            return 2;
        }

        [NoAutomaticTrigger]
        public static void SyncBranchResourceFiles()
        {
            var proc = new SyncProcessor(new DefaultResxLoader());
            proc.LoadConfiguration();

            proc.Run(stopOnFirstError: false);
        }
    }
}
