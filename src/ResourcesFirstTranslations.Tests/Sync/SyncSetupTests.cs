using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using ResourcesFirstTranslations.Data;
using ResourcesFirstTranslations.Services;
using ResourcesFirstTranslations.Sync;

namespace ResourcesFirstTranslations.Tests.Sync
{
    [TestFixture]
    public class SyncSetupTests
    {
        // https://github.com/Moq/moq4
        // http://msdn.microsoft.com/en-us/data/dn314429.aspx

        [Test]
        public void TestLoadingConfiguration()
        {
            var mockLoader = new Mock<IResxLoader>();

            var languageData = new List<Language> 
            { 
                new Language { Culture = "de" }, 
                new Language { Culture = "es" }, 
            }; 
            var mockLanguages = new Mock<DbSet<Language>>();
            EfMoqHelpers.SetupIQueryable(mockLanguages, languageData);
            
            var branchData = new List<Branch> 
            { 
                new Branch { Id = 400 }
            }; 
            var mockBranches = new Mock<DbSet<Branch>>();
            EfMoqHelpers.SetupIQueryable(mockBranches, branchData);

            var branchResourceFiles = new List<BranchResourceFile>(); // empty list
            var mockBranchResourceFiles = new Mock<DbSet<BranchResourceFile>>();
            EfMoqHelpers.SetupIQueryable(mockBranchResourceFiles, branchResourceFiles);

            var mockContext = new Mock<RftContext>();
            mockContext.Setup(m => m.Languages).Returns(mockLanguages.Object);
            mockContext.Setup(m => m.Branches).Returns(mockBranches.Object);
            mockContext.Setup(m => m.BranchResourceFiles).Returns(mockBranchResourceFiles.Object);

            var p = new SyncProcessor(mockLoader.Object, mockContext.Object);
            p.LoadConfiguration();
     
            Assert.That(p.Languages, Is.Not.Null);
            Assert.That(p.Languages.Count, Is.EqualTo(2));

            Assert.That(p.Branches, Is.Not.Null);
            Assert.That(p.Branches.Count, Is.EqualTo(1));
        }

        [Test]
        public void TestOnEmptyDatabase()
        {
            var mockLoader = new Mock<IResxLoader>();

            var languageData = new List<Language>();
            var mockLanguages = new Mock<DbSet<Language>>();
            EfMoqHelpers.SetupIQueryable(mockLanguages, languageData);

            var branchData = new List<Branch>();
            var mockBranches = new Mock<DbSet<Branch>>();
            EfMoqHelpers.SetupIQueryable(mockBranches, branchData);

            var branchResourceFiles = new List<BranchResourceFile>();
            var mockBranchResourceFiles = new Mock<DbSet<BranchResourceFile>>();
            EfMoqHelpers.SetupIQueryable(mockBranchResourceFiles, branchResourceFiles);

            var mockContext = new Mock<RftContext>();
            mockContext.Setup(m => m.Languages).Returns(mockLanguages.Object);
            mockContext.Setup(m => m.Branches).Returns(mockBranches.Object);
            mockContext.Setup(m => m.BranchResourceFiles).Returns(mockBranchResourceFiles.Object);

            var p = new SyncProcessor(mockLoader.Object, mockContext.Object);
            p.LoadConfiguration();
            bool result = p.Run(true);

            Assert.That(result, Is.True);
            Assert.That(p.Branches.Count, Is.EqualTo(0));
        }
    }
}
