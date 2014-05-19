using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ResourcesFirstTranslations.Resx;

namespace ResourcesFirstTranslations.Tests.Resx
{
    [TestFixture]
    public class ResourceReaderTests
    {
        [Test]
        public void LoadAllResxFileTest()
        {
            var rr = new ResxStringResourceReader();
            var resxFileContent = new StringReader(ResourceFilesLoader.Load(ResourceFilesLoader.DefaultResources));

            var result = rr.ResxToResourceStringDictionary(resxFileContent);

            Assert.That(result.Count, Is.EqualTo(2515));
        }

        [Test]
        public void LoadSingleWithCommentResxFileTest()
        {
            var rr = new ResxStringResourceReader();
            var resxFileContent = new StringReader(ResourceFilesLoader.Load(ResourceFilesLoader.SingleResourceWithComment));

            var result = rr.ResxToResourceStringDictionary(resxFileContent);

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.First().Comment, Is.EqualTo("Some comment has to be here"));
        }
    }
}
