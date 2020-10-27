using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using Tandem.Common.StatusResponse.Infrastructure;
using Tandem.Web.Apps.Trivia.Adapter.Contracts;
using Tandem.Web.Apps.Trivia.Adapter.Impl;
using Tandem.Web.Apps.Trivia.Data;
using Tandem.Web.Apps.Trivia.Facade.Contracts;
using Tandem.Web.Apps.Trivia.Facade.Impl;
using Tandem.Web.Apps.Trivia.Facade.Translators;
using Tandem.Web.Apps.Trivia.WebService.Controllers.Translators;
using SC = Tandem.Web.Apps.Trivia.Infrastructure.SystemConstants;
using AppSettings = Tandem.Web.Apps.Trivia.Infrastructure.SystemConstants.AppSettings;
using Tandem.Web.Apps.Trivia.WebService.Middleware.TokenValidation;

namespace Tandem.Web.Apps.Trivia.WebService
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
            #region FRAMEWORK SERVICES
            services.AddControllersWithViews();
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
            services.AddHttpContextAccessor();
            #endregion

            #region ADAPTERS
            services.AddScoped<ITriviaAdapter, TriviaAdapter>();
            services.AddScoped<IPlayerAdapter, PlayerAdapter>();
            #endregion

            #region FACADES
            services.AddScoped<ITriviaFacade, TriviaFacade>();
            services.AddScoped<IPlayerFacade, PlayerFacade>();
            #endregion

            #region INTERNAL SERVICES
            services.AddTriviaDataService($"{Directory.GetCurrentDirectory()}\\{Configuration.GetConnectionString(AppSettings.ConnStrings.DataFilePath)}");
            services.AddStatusResponse();
            #endregion

            #region EXTERNAL SERVICES
            services.AddAutoMapper(
                typeof(Entity_BusinessEntity),
                typeof(BusinessModel_BusinessEntity)
            );

            services.AddOpenApiDocument(cfg =>
            {
                cfg.SchemaType = NJsonSchema.SchemaType.OpenApi3;
                cfg.Title = "Tandem.Web.Apps.Trivia";
            });
            #endregion

            #region MISC
            services.SetTokenValidationPathsToExclude();
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}

            app.MapWhen(
                path => path.Request.Path.Value?.ToLower() == "/status/isalive", // When request path is /status/isalive.
                builder => builder.Run(async context => await context.Response.WriteAsync($"Trivia SPA server is currently running.")) // Return this message.
            );
            app.UseTokenValidationMiddleware();
            app.UseStatusResponseMiddleware();

            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseOpenApi(); //Generates OpenAPI-compliant schema for API
            app.UseSwaggerUi3(); //Generates UI that parses generated schema

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    //spa.UseAngularCliServer(npmScript: "start");
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                }
            });
        }
    }
}
