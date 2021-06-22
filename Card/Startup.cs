using Camunda.Worker;
using Camunda.Worker.Client;
using Card.ExternalHandlers;
using Card.Models;
using Card.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace Card
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            var appSettings = appSettingsSection.Get<AppSettings>();

            services.AddSingleton<IBPMNService, BPMNService>(_ => new BPMNService(appSettings.CamundaRestApiUri));
            services.AddSingleton<IOpenCardService, OpenCardService>();
            

            services.AddControllersWithViews();

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });

            // Конфигурируем подключенеие к Camunda
            services.AddExternalTaskClient()
                .ConfigureHttpClient((provider, client) =>
                {
                    client.BaseAddress = new Uri("http://localhost:8080/engine-rest"); // Регестрируем подключение к Camunda Engine
                });

            // Конфигурируем обработчик
            services.AddCamundaWorker("OpenCardWorker", 10)
                .AddHandler<FrontOfficeCheckRequestHandler>()
                .AddHandler<FrontOfficeCheckResultHandler>()
                .AddHandler<BlacklistRequestHandler>()
                .AddHandler<BlacklistResultedHandler>()
                .AddHandler<OpenCardBlackListHandler>()
                .ConfigurePipeline(pipeline =>
                {
                    pipeline.Use(next => async context =>
                    {
                        var logger = context.ServiceProvider.GetRequiredService<ILogger<Startup>>(); // Использовать систему логирования 
                        logger.LogInformation("Начала обработки таска {id}", context.Task.Id); // Записать в лог информацию
                        await next(context); // Ожидание следующего запроса
                        logger.LogInformation("Начала обработки таска {id}", context.Task.Id); // Записать в лог информацию
                    });
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
