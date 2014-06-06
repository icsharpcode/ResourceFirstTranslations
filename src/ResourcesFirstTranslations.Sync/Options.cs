using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;

namespace ResourcesFirstTranslations.SyncJob
{
    // Running locally, not as a WebJob (with automatic entry points)
    // sync.exe -webjob false

    class Options
    {
        [Option('j', "jobtype", Required = false, DefaultValue = "azure", HelpText = "'local' or 'azure'")]
        public string JobType { get; set; }
        
        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this,
              (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
