using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;
using LegacyApp.Ordering.Application;
using LegacyApp.Ordering.Domain;
using LegacyApp.Ordering.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace LegacyApp.Ordering.Api
{
    public class Startup
    {
        public static void Bootstrapper(HttpConfiguration config)
        {
            var provider = Configuration();
            var resolver = new DefaultDependencyResolver(provider);

            config.DependencyResolver = resolver;
        }

        private static IServiceProvider Configuration()
        {
            var services = new ServiceCollection();

            services.AddControllersAsServices(typeof(Startup).Assembly.GetExportedTypes()
                .Where(t => !t.IsAbstract && !t.IsGenericTypeDefinition)
                .Where(t => typeof(IHttpController).IsAssignableFrom(t)
                            || t.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase)));

            services.AddScoped<LegacyAppDbContext, LegacyAppDbContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<IGetCartQuery, GetCartQuery>();

            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider;
        }
    }
}