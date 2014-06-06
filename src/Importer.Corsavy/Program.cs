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

        // NOTE: Languages > 2k missing are removed, statistics as of 6/6/2014
        //   Urdu 	urdu 	2538
        //   Afrikaans 	af 	2537
        //   Indonesian 	id 	2534
        //   Persian 	fa 	2492
        //   Thai 	th 	2491
        //   Vietnamese 	vi 	2406
        //   Goiserisch 	goisern 	2393
        //   Belarusian 	be 	2366
        //   Croatian 	hr 	2351
        //   Hebrew 	he 	2343
        //   Arabic 	ar 	2254
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
