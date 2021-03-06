using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Api;
using Library.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using Library.Identity;

namespace Library
{
    public class Configuration
    {
        public static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
        {
            services.AddSingleton<IArticleService, ArticleService>();
            services.AddSingleton<ILaunchService, LaunchService>();
            services.AddSingleton<IRocketService, RocketService>();
            services.AddSingleton<IEventService, EventService>();
            services.AddScoped<ISaveService, SaveService>();

            services.AddRefitClient<IArticleApi>()
                .ConfigureHttpClient(httpClient =>
                {
                    httpClient.BaseAddress = new Uri(configuration["Api:Article:BaseAddress"]);
                });

            services.AddRefitClient<ILaunchApi>()
                .ConfigureHttpClient(httpClient =>
                {
                    httpClient.BaseAddress = new Uri(configuration["Api:Launch:BaseAddress"]);
                });
        }
    }
}
