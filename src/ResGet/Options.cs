using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;

namespace ResGet
{
    // Sample usages we are shooting for:

    // #1: Will get all the resource files for all languages and overwrite existing files
    // resget -url http://t/resources -targetDir "..\resources" -branch 400 -overwrite true

    // #2: Will get all the resource files for German, does not overwrite existing files
    // resget -url http://t/resources -targetDir "..\resources" -branch 400 -language de

    // -branch and -url are mandatory, -targetDir defaults to current directory, -overwrite to false
    // When -language is missing, all languages will be retrieved

    class Options
    {
        [Option('u', "url", Required = true, HelpText = "Url to the /Resources/ controller. Trailing slash mandatory.")]
        public string Url { get; set; }
        
        [Option('b', "branch", Required = true, HelpText = "Branch Id (numeric).")]
        public int Branch { get; set; }

        [Option('t', "targetDir", Required = false, HelpText = "Target directory.")]
        public string TargetDirectory { get; set; }

        [Option('l', "language", Required = false, HelpText = "Get a specific language.")]
        public string Language { get; set; }

        [Option('o', "overwrite", Required = false, DefaultValue = false, HelpText = "Overwrite existing files.")]
        public bool Overwrite { get; set; }

        [Option('f', "format", Required = false, DefaultValue = "resources", HelpText = "Format: resources or resx.")]
        public string Format { get; set; }

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
