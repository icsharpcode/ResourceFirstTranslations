using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ResourcesFirstTranslations.Data;

namespace ResourcesFirstTranslations.Sync
{
    // Aktuell nur eine Idee
    public class SyncContextWrapper : ISyncContextWrapper
    {
        public SyncContextWrapper()
        {
            ctx = new RftContext();
        }

        private readonly RftContext ctx;


        public void RemoveResourceStrings(List<ResourceString> toRemove)
        {
            
        }

        public int SaveChanges()
        {
            return ctx.SaveChanges();
        }
    }
}
