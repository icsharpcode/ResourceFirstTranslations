using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ResourcesFirstTranslations.Web.Startup))]
namespace ResourcesFirstTranslations.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
