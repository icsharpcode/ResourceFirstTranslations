using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourcesFirstTranslations.Tests
{
    public static class ResourceFilesLoader
    {
        public static string Load(string filename)
        {
            return System.IO.File.ReadAllText(filename);
        }

        public const string DefaultResources = "StringResources.resx";
        public const string SingleResourceWithComment = "SingleResourceWithComment.resx";
        public const string EmptyStringResources = "StringResourcesEmpty.resx";
    }
}
