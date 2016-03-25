using System;

using Microsoft.AspNet.Authentication.JwtBearer;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Serilog;

using SSW.MusicStore.API.Services;

using System.Threading.Tasks;

using Autofac;
using Autofac.Extensions.DependencyInjection;

using SerilogWeb.Classic.Enrichers;

using Microsoft.Extensions.PlatformAbstractions;
using SSW.MusicStore.API.Filters;
using SSW.MusicStore.Data;
using SSW.MusicStore.Data.Initializers;
using SSW.MusicStore.DependencyResolution;

namespace SSW.MusicStore.API
{
    public class Startup
    {
        public Startup(IHostingEnvironment env, IApplicationEnvironment appEnv)
        {

            var builder = new ConfigurationBuilder()
                .SetBasePath(appEnv.ApplicationBasePath);

            if (env.IsDevelopment())
            {
                builder.AddJsonFile("appsettings.json").AddJsonFile("privatesettings.json");
            }
            else
            {
                builder.AddJsonFile("appsettings.json", optional: true);
            }

            builder.AddUserSecrets();
            builder.AddEnvironmentVariables();
            this.Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            services
                .AddMvc()
                .AddJsonOptions(
                    options =>
                        {
                            options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                        })
                .AddMvcOptions(
                    options =>
                    {
                        options.Filters.Add(new MvcLogActionFilter());
                        options.Filters.Add(new MvcExceptionActionFilter());
                        options.Filters.Add(new MvcValidateModelActionFilter());
                    });

            var builder = new ContainerBuilder();

            // Load web specific dependencies
            builder.RegisterType<AuthMessageSender>()
                .As<IEmailSender>().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(typeof(Startup).Assembly).AsImplementedInterfaces();

            var databaseInitializer = new MigrateToLatestVersion(new SampleDataSeeder());
            builder.RegisterModule(
                new DataModule(this.Configuration["Data:DefaultConnection:ConnectionString"], databaseInitializer));

            // Populate the container with services that were previously registered
            builder.Populate(services);

            var container = builder.Build();

            return container.Resolve<IServiceProvider>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            var config =
                new LoggerConfiguration()
                    .WriteTo.ColoredConsole()
                    .WriteTo.Seq(serverUrl: Configuration["Seq:Url"], apiKey: Configuration["Seq:Key"])
                    .Enrich.WithProperty("ApplicationName", "Music Store")
                    .Enrich.With(new HttpRequestIdEnricher());
            Log.Logger = config.CreateLogger();

            loggerFactory.MinimumLevel = LogLevel.Information;
            loggerFactory.AddSerilog();
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseCors(policy => policy
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            }
            else
            {
                app.UseCors(policy => policy
                            .WithOrigins(Configuration["Cors:Url"])
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials());
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseIISPlatformHandler();

            app.UseStaticFiles();

            app.UseJwtBearerAuthentication(options =>
            {
                options.Audience = Configuration["Auth0:ClientId"];
                options.Authority = $"https://{Configuration["Auth0:Domain"]}";
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        Log.Logger.Error("Authentication failed.", context.Exception);
                        return Task.FromResult(0);
                    }
                };
            });

            // Note: this line must be after the OAuth config above
            app.UseMvc();

        }
    }
}
