using System;

using Microsoft.AspNet.Authentication.JwtBearer;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Serilog;
using System.Threading.Tasks;

using Autofac;
using SerilogWeb.Classic.Enrichers;

using Microsoft.Extensions.PlatformAbstractions;
using SSW.MusicStore.API.Filters;
using SSW.MusicStore.API.Infrastructure.DI;
using Swashbuckle.SwaggerGen;

using Mindscape.Raygun4Net.AspNetCore;
using SSW.MusicStore.API.Settings;

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
                builder.AddJsonFile("appsettings.json").AddJsonFile("privatesettings.json", optional: true);
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
            services.Configure<AppSettings>(options => Configuration.Bind(options));

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
                        options.Filters.Add(new MvcExceptionActionFilter(this.Configuration));
                        options.Filters.Add(new MvcValidateModelActionFilter());
                    });

            services.AddRaygun(this.Configuration);
            services.AddApplicationInsightsTelemetry(this.Configuration);

            RegisterSwagger(services);

            var container = services.CreateAutofacContainer(this.Configuration);

            return container.Resolve<IServiceProvider>();
        }

        private static void RegisterSwagger(IServiceCollection services)
        {
            services.ConfigureSwaggerDocument(opt =>
            {
                opt.SingleApiVersion(new Info
                {
                    Version = "v1",
                    Title = "Music Store API",
                    Description = "API for SSW Music Store"
                });
            });
            services.ConfigureSwaggerSchema(opt => { opt.DescribeAllEnumsAsStrings = true; });
            services.AddSwaggerGen();
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
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "",
                    defaults: new { controller = "Home", action = "Index" }
                );
            });
            app.UseSwaggerUi();
            app.UseSwaggerGen();

            app.UseRaygun();

            app.UseApplicationInsightsRequestTelemetry();
            app.UseApplicationInsightsExceptionTelemetry();
        }
    }
}
