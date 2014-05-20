using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.Entity.Core.EntityClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourcesFirstTranslations.Data
{
    public partial class RftContext
    {
        public RftContext(DbConnection existingConnection, bool contextOwnsConnection)
            : base(existingConnection, contextOwnsConnection)
        {
        }

        // ctx.Database.Connection.ConnectionString strips the password, not useful in Azure
        // Therefore we have to use the builder
        public string GetProviderConnectionString()
        {
            string efConnectionString = ConfigurationManager.ConnectionStrings["RftContext"].ConnectionString;

            var builder = new EntityConnectionStringBuilder(efConnectionString);
            return builder.ProviderConnectionString;
        }
    }
}
