using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using ResourcesFirstTranslations.Services;
using ResourcesFirstTranslations.Services.Stubs;

namespace ResourcesFirstTranslations.Web.App_Start
{
    public static class AutoFacConfig
    {
        public static void Configure()
        {
            // http://code.google.com/p/autofac/wiki/MvcIntegration
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            builder.Register(c => new DefaultConfigurationService()).As<IConfigurationService>().InstancePerDependency();
            builder.Register(c => new DefaultDataService()).As<IDataService>().InstancePerDependency();
            builder.Register(c => new DefaultCacheService()).As<ICacheService>().InstancePerDependency();

            // Enable the desired mail service (not by type name but by friendly name)
            string selectedMailService = new DefaultConfigurationService().MailService;
            if ("sendgrid" == selectedMailService)
            {
                builder.Register(c => new SendGridMailService(c.Resolve<IConfigurationService>()))
                    .As<IMailService>().InstancePerDependency();
            }
            else if ("smtp" == selectedMailService)
            {
                builder.Register(c => new SmtpMailService(c.Resolve<IConfigurationService>()))
                    .As<IMailService>().InstancePerDependency();
            }
            else
            {
                builder.Register(c => new DevStubMailService()).As<IMailService>().InstancePerDependency();
            }


            builder.Register(c => new DefaultTranslationService(c.Resolve<IDataService>(), 
                    c.Resolve<ICacheService>(),
                    c.Resolve<IMailService>(),
                    c.Resolve<IConfigurationService>()))
                .As<ITranslationService>().InstancePerDependency();

            // MVC
            IContainer container = builder.Build();
            System.Web.Mvc.DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            // Web API
            var resolver = new Autofac.Integration.WebApi.AutofacWebApiDependencyResolver(container);
            System.Web.Http.GlobalConfiguration.Configuration.DependencyResolver = resolver;
        }
    }
}