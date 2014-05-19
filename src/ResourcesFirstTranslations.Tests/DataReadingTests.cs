using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using NUnit.Framework;
using ResourcesFirstTranslations.Data;
using ResourcesFirstTranslations.Services;

namespace ResourcesFirstTranslations.Tests
{
    [TestFixture]
    public class DataReadingTests
    {
        [Test]
        public void ReadFromBranch500()
        {
            using (var txScope = new TransactionScope())
            {
                using (var ctx = new RftContext())
                {
                    var translations = ctx.Translations.Where(t => t.FK_BranchId == 500).ToList();
                    Assert.That(translations.Count, Is.EqualTo(2));

                    //ctx.Translations.RemoveRange(translations);
                    //ctx.SaveChanges();
                }
            }
        }
    }
}
