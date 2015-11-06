using Microsoft.AspNet.Authentication.JwtBearer;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Diagnostics.Entity;
using Microsoft.AspNet.Hosting;
using Microsoft.Data.Entity;
using Microsoft.Dnx.Runtime;
using Microsoft.Framework.Configuration;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;
using Newtonsoft.Json.Serialization;
using Serilog;
using SSW.MusicStore.API.Models;
using SSW.MusicStore.API.Services;
using SSW.MusicStore.API.Services.Query;
using System.Threading.Tasks;

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

            builder.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
            builder.AddUserSecrets();
            builder.AddEnvironmentVariables();
            Configuration = builder.Build();

        }

		public IConfigurationRoot Configuration { get; set; }

		public void ConfigureServices(IServiceCollection services)
		{
            services.AddEntityFramework()
                .AddSqlServer()
                .AddDbContext<MusicStoreContext>(options =>
                options.UseSqlServer(Configuration["Data:DefaultConnection:ConnectionString"]));

            services.AddCors();

            services.AddMvc()
        .AddJsonOptions(opt =>
    {
        opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
    });

            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<IDbContextFactory<MusicStoreContext>, DbContextFactory>();
            services.AddTransient<IGenreQueryService, GenreQueryService>();
            services.AddTransient<IAlbumQueryService, AlbumQueryService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
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
                app.UseDatabaseErrorPage(DatabaseErrorPageOptions.ShowAll);
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseIISPlatformHandler();

            app.UseStaticFiles();

            app.UseMvc();
            
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
                    },
                };
            });

            //Slows web api - only do this on first run to popular db
            //SampleData.InitializeMusicStoreDatabaseAsync(app.ApplicationServices).Wait();

        }
    }
}
