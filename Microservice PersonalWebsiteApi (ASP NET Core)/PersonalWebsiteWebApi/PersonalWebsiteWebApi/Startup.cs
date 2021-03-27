using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using PersonalWebsiteWebApi.DatabaseContext;
using PersonalWebsiteWebApi.Repositories;
using PersonalWebsiteWebApi.Services;
using PersonalWebsiteWebApi.Settings;
using System;
using System.IO;
using System.Text;

namespace PersonalWebsiteWebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //Settings
            services.Configure<GithubSettings>(Configuration.GetSection("GithubSettings"));
     
            //Database
            services.AddDbContext<PersonalWebsiteContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("PersonalWebsite")));

            //Repositories
            services.AddScoped<IGithubProjectRepository, GithubProjectRepository>();
            services.AddScoped<IGalleryImageRepository, GalleryImageRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            //Services
            services.AddTransient<IGithubProjectUpdaterService, GithubProjectUpdaterService>();
            services.AddTransient<IImageFileHandlerService, ImageFileHandlerService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();

            //Cross-Origin Resource Sharing
            services.AddCors(o => o.AddPolicy("DefaultPolicy", builder =>
            {
                builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
            }));

            //Hangfire Job Server
            services.AddHangfire(conf => 
                conf.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseDefaultTypeSerializer()
                .UseMemoryStorage());

            //JWT Authentication
            var jsonWebTokenSection = Configuration.GetSection("JsonWebTokenSettings");
            services.Configure<JsonWebTokenSettings>(jsonWebTokenSection);

            var jsonWebTokenConfig = jsonWebTokenSection.Get<JsonWebTokenSettings>();
            var key = Encoding.ASCII.GetBytes(jsonWebTokenConfig.Secret);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            //HttpClient
            services.AddHttpClient();

            //Controllers
            services.AddControllers();
        }

        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            IRecurringJobManager recurringJobManager,
            IBackgroundJobClient backgroundJobClient,
            IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("DefaultPolicy");

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "StaticImages")),
                RequestPath = "/cds/images"
            });

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //Hangfire Server
            app.UseHangfireServer();

            //Fire-and-forget
            backgroundJobClient.Schedule(() => serviceProvider.GetService<IGithubProjectUpdaterService>().Update(), TimeSpan.FromSeconds(30));

            //Daily jobs
            recurringJobManager.AddOrUpdate(
                "Update Github projects",
                () => serviceProvider.GetService<IGithubProjectUpdaterService>().Update(),
                Cron.Daily,
                TimeZoneInfo.Local);
        }
    }
}
