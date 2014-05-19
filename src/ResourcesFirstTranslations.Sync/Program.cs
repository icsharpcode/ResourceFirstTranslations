// It is your choice to either build for the Azure Web Jobs Runtime or for a local scheduled task
#define USEAZUREWEBJOB

using System;
using Microsoft.WindowsAzure.Jobs;
using ResourcesFirstTranslations.Services;
using ResourcesFirstTranslations.Sync;

namespace ResourcesFirstTranslations.SyncJob
{
    class Program
    {
        static void Main(string[] args)
        {
#if USEAZUREWEBJOB
            JobHost host = new JobHost();
            host.Call(typeof(Program).GetMethod("SyncBranchResourceFiles"));
#else
            SyncBranchResourceFiles();
#endif
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
