using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ResourcesFirstTranslations.Data;

namespace Importer.Corsavy
{
    public class Importer
    {
        private readonly string _connectionString;
        private readonly List<string> _ignoreOnImport;

        public Importer(string connectionString, List<string> ignoreOnImport)
        {
            _connectionString = connectionString;
            _ignoreOnImport = ignoreOnImport;
        }

        public List<Language> GetLanguages()
        {
            var langList = new List<Language>();

            var oldIsoCodes = ConfigAsp.IsoCodes;
            var oldLangNames = ConfigAsp.LanguageNames;

            for (int i = 0; i < oldIsoCodes.Count; i++)
            {
                string isoCode = oldIsoCodes[i];
                string langName = oldLangNames[i];

                if (CanImport(isoCode))
                {
                    langList.Add(new Language()
                    {
                        Culture = isoCode,
                        Description = langName
                    });
                }
            }

            return langList;
        }

        private bool CanImport(string isoCode)
        {
            return !_ignoreOnImport.Contains(isoCode);
        }

        public void PrepareBranchesAndLanguages()
        {
            using (var ctx = new RftContext())
            {
                ctx.Languages.AddRange(GetLanguages());

                ctx.ResourceFiles.AddRange(new List<ResourceFile>
                {
                    new ResourceFile()
                    {
                        Id = IdConstants.ResourceStringResources,
                        ResourceFileDisplayName = "StringResources",
                        ResourceFileNameFormat = "StringResources.{0}.resources"
                    },
                });

                ctx.Branches.AddRange(new List<Branch>
                {
                    new Branch()  // our most current branch
                    {
                        Id = IdConstants.Branch5,
                        BranchDisplayName = "5.x",
                        BranchRootUrl = "https://github.com/icsharpcode/SharpDevelop/tree/master/"
                    },
                    new Branch()  // our legacy branch
                    {
                        Id = IdConstants.Branch4, 
                        BranchDisplayName = "4.x",
                        BranchRootUrl = "https://github.com/icsharpcode/SharpDevelop/tree/4.x"
                    },
                });

                // Save the languages, branches and resource files - then add the branch resource files
                ctx.SaveChanges();

                ctx.BranchResourceFiles.AddRange(new List<BranchResourceFile>
                {
                    new BranchResourceFile()
                    {
                        FK_BranchId = IdConstants.Branch5,
                        FK_ResourceFileId = IdConstants.ResourceStringResources,
                        SyncRawPathAbsolute = "https://github.com/icsharpcode/SharpDevelop/raw/master/data/resources/StringResources.resx"
                    },
                    new BranchResourceFile()
                    {
                        FK_BranchId = IdConstants.Branch4,
                        FK_ResourceFileId = IdConstants.ResourceStringResources,
                        SyncRawPathAbsolute = "https://github.com/icsharpcode/SharpDevelop/raw/4.x/data/resources/StringResources.resx"
                    },
                });

                ctx.SaveChanges();
            }
        }

        public void Import()
        {
            var oldIsoCodes = ConfigAsp.IsoCodes;
            var oldIsoCodesCount = oldIsoCodes.Count;
            string queryString = "SELECT * FROM Localization";

            var resources = new List<ResourceString>();
            var translations = new List<Translation>();
            var dt = DateTime.Now.Date;

            using (var connection = new OleDbConnection(_connectionString))
            using (var command = new OleDbCommand(queryString, connection))
            {
                try
                {
                    connection.Open();
                    OleDbDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Console.WriteLine(reader[0].ToString());

                        string resourceName = OleDbToString(reader["ResourceName"]);
                        string originalValue = OleDbToString(reader["PrimaryResLangValue"]);
                        DateTime originalLastModified = OleDbPrimaryLastModifiedToSqlDateTime(reader["PrimaryLastModified"]);
                        string comment = OleDbToString(reader["PrimaryPurpose"]);

                        resources.Add(new ResourceString()
                        {
                            FK_BranchId = IdConstants.Branch5,
                            FK_ResourceFileId = IdConstants.ResourceStringResources,
                            ResourceIdentifier = resourceName,
                            ResxValue = originalValue,
                            ResxComment = comment,
                            LastUpdatedFromRepository = originalLastModified
                        });

                        for (int i = 0; i < oldIsoCodesCount; i++)
                        {
                            string isoCode = oldIsoCodes[i];
                            if (CanImport(isoCode))
                            {
                                string translatedValue = OleDbToString(reader["lang-" + isoCode]);
                                bool translationIsDirty = IsDirtyOleDbToBool(reader["dirty-lang-" + isoCode]);

                                translations.AddRange(IdConstants.BranchIds.Select(branchId => new Translation()
                                {
                                    FK_BranchId = branchId, 
                                    FK_ResourceFileId = IdConstants.ResourceStringResources, 
                                    ResourceIdentifier = resourceName, 
                                    Culture = isoCode, 
                                    TranslatedValue = translatedValue, 
                                    OriginalResxValueChangedSinceTranslation = translationIsDirty, 
                                    LastUpdated = dt, 
                                    LastUpdatedBy = "import", 
                                    OriginalResxValueAtTranslation = originalValue
                                }));
                            }
                        }
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception reading source database!\r\n" + ex.ToString());

                    resources.Clear();
                    translations.Clear();
                }

                // Now store the parsed import data to the new database
                try
                {
                    using (var ctx = new RftContext())
                    {
                        var connectionString = ctx.Database.Connection.ConnectionString;

                        // EF is really, really slow (esp. when going to Azure)
                        //ctx.ResourceStrings.AddRange(resources);
                        //ctx.SaveChanges();

                        //ctx.Translations.AddRange(translations);
                        //ctx.SaveChanges();

                        // Option #2: This fails with https://efbulkinsert.codeplex.com/workitem/1383
                        //var options = new BulkInsertOptions
                        //{
                        //    EnableStreaming = true,
                        //};
                        //ctx.BulkInsert(translations, options);

                        // Bulk insert via https://github.com/ronnieoverby/RonnieOverbyGrabBag/wiki/Bulk-Inserter
                        using (var sqlConnection = new SqlConnection(connectionString))
                        {
                            sqlConnection.Open();
                            const int bufferSize = 5000;

                            Trace.TraceInformation("Saving {0} resource strings to the database", resources.Count);

                            var bcpR = new Overby.Data.BulkInserter<ResourceString>(sqlConnection, "ResourceStrings", bufferSize);
                            bcpR.Insert(resources);

                            Trace.TraceInformation("Saving {0} translations to the database (this will take long)", translations.Count);

                            var bcpT = new Overby.Data.BulkInserter<Translation>(sqlConnection, "Translations", bufferSize);
                            bcpT.Insert(translations);
                        }

                        Trace.TraceInformation("Storing resource strings and translations succeeded");
                    }
                }
                catch (Exception ex)
                {
                    Trace.TraceError("Exception storing import data!\r\n" + ex.ToString());
                }
            }
        }

        private static string OleDbToString(object o)
        {
            if (o == DBNull.Value || o == null)
                return "";

            return o.ToString();
        }

        private static DateTime OleDbPrimaryLastModifiedToSqlDateTime(object o)
        {
            // default(DateTime) would generate a value not consumable for datetime (only datetime2) data type
            // thus we set the value to the start date of the project so we have a recognizeable date
            if (o == DBNull.Value || o == null)
                return new DateTime(2000,9,11);
            
            return Convert.ToDateTime(o);
        }

        private static bool IsDirtyOleDbToBool(object o)
        {
            if (o == DBNull.Value || o == null)
                return true;

            return Convert.ToBoolean(o);
        }
    }
}
