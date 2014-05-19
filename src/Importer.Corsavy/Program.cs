using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ResourcesFirstTranslations.Data;

namespace Importer.Corsavy
{
    class Program
    {
        private const string ConnectionString = 
                        @"Provider=Microsoft.Jet.OLEDB.4.0;" + 
                        @"Data Source=LocalizeDb_DL_Corsavy.mdb;" + 
                        @"User Id=;Password=;";

        // TODO: which languages do we import? (atm > 2k missing are removed)
        private static readonly List<string> IgnoreOnImport = new List<string>()
        {
            "en", "goisern", // en isn't really in the database, goisern was a fake translation
            "urdu", "af", "id", "fa", "th", "vi", "be", "hr", "he", "ar"
        }; 

        // TODO: all branches or only 5.x?

        static void Main(string[] args)
        {
            // Note: database must be entirely empty before running this!

            var importer = new Importer(ConnectionString, IgnoreOnImport);

            try
            {
                Trace.TraceInformation("Create branches and languages...");
                importer.PrepareBranchesAndLanguages();

                Trace.TraceInformation("Import existing resource strings...");
                importer.Import();
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
            }

            Console.WriteLine("Done. Press any key to continue...");
            Console.ReadLine();
        }
    }
}
