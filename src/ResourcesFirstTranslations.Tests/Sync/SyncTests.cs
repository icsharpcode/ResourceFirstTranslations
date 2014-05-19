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
    public class SyncTests
    {
        private Mock<IResxLoader> GetEmptyResxFileLoader(string onPath)
        {
            var mockResourceFileLoader = new Mock<IResxLoader>();
            mockResourceFileLoader.Setup(l => l.GetAsString(onPath, true))
                .Returns(ResourceFilesLoader.Load(ResourceFilesLoader.EmptyStringResources));
            return mockResourceFileLoader;
        }

        private Mock<RftContext> GetSingleBranchSingleLanguageNoResourcesContextMock()
        {
            var languageData = new List<Language> 
            { 
                new Language { Culture = "de" }, 
            };
            var mockLanguages = new Mock<DbSet<Language>>();
            EfMoqHelpers.SetupIQueryable(mockLanguages, languageData);

            var branchData = new List<Branch> 
            { 
                new Branch { Id = 400 }
            };
            var mockBranches = new Mock<DbSet<Branch>>();
            EfMoqHelpers.SetupIQueryable(mockBranches, branchData);

            var branchResourceFiles = new List<BranchResourceFile>()
            {
                new BranchResourceFile()
                {
                    FK_BranchId = 400,
                    FK_ResourceFileId = 4711,
                    SyncRawPathAbsolute = "gohere"
                }
            };
            var mockBranchResourceFiles = new Mock<DbSet<BranchResourceFile>>();
            EfMoqHelpers.SetupIQueryable(mockBranchResourceFiles, branchResourceFiles);

            var resourceStrings = new List<ResourceString>();
            var mockResourceStrings = new Mock<DbSet<ResourceString>>();
            EfMoqHelpers.SetupIQueryable(mockResourceStrings, resourceStrings);

            var mockContext = new Mock<RftContext>();
            mockContext.Setup(m => m.Languages).Returns(mockLanguages.Object);
            mockContext.Setup(m => m.Branches).Returns(mockBranches.Object);
            mockContext.Setup(m => m.BranchResourceFiles).Returns(mockBranchResourceFiles.Object);
            mockContext.Setup(m => m.ResourceStrings).Returns(mockResourceStrings.Object);
            
            return mockContext;
        }

        [Test]
        public void EmptyResourceFileOnEmptyDatabase()
        {
            var mockContext = GetSingleBranchSingleLanguageNoResourcesContextMock();
            var mockLoader = GetEmptyResxFileLoader("gohere");

            var p = new SyncProcessor(mockLoader.Object, mockContext.Object);
            p.LoadConfiguration();
            bool result = p.Run(true);

            Assert.That(result, Is.True);
        }
    }
}
