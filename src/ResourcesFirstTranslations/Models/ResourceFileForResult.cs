using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourcesFirstTranslations.Models
{
    public class ResourceFileForResult
    {
        public MemoryStream Stream { get; set; }
        public bool Succeeded { get; set; }
        public string Filename { get; set; }
    }
}
