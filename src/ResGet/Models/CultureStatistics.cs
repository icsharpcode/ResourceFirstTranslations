using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResGet.Models
{
    class CultureStatistics
    {
        public string Culture { get; set; }
        public string CultureDisplayName { get; set; }
        public int MissingTranslations { get; set; }
    }
}
