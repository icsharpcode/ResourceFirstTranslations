using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourcesFirstTranslations.Services
{
    public interface IResxLoader
    {
        Task<string> GetAsStringAsync(string url);
        string GetAsString(string url, bool suppressExceptions = true);
    }
}
