using System.Web.Mvc;

namespace ResourcesFirstTranslations.Web.Areas.Administration
{
    public class AdministrationAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Administration";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Administration_default",
                "Administration/{controller}/{action}/{id}",
                new { controller="AdminstrationHome", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}