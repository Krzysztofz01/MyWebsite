using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PersonalWebsiteWebApi.DatabaseContext;
using PersonalWebsiteWebApi.Repositories;
using PersonalWebsiteWebApi.Services;
using PersonalWebsiteWebApi.Settings;
using System;

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
            services.Configure<CdsSettings>(Configuration.GetSection("CdsSettings"));

            //Database
            services.AddDbContext<PersonalWebsiteContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("PersonalWebsite")));

            //Repositories
            services.AddScoped<IGithubProjectRepository, GithubProjectRepository>();
            services.AddScoped<IGalleryImageRepository, GalleryImageRepository>();

            //Services
            services.AddTransient<IGithubProjectUpdaterService, GithubProjectUpdaterService>();
            services.AddTransient<IGalleryIndexerService, GalleryIndexerService>();

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

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //Hangfire Server
            app.UseHangfireServer();

            //Fire-and-forget
            backgroundJobClient.Schedule(() => serviceProvider.GetService<IGithubProjectUpdaterService>().Update(), TimeSpan.FromMinutes(2));
            backgroundJobClient.Schedule(() => serviceProvider.GetService<IGalleryIndexerService>().IndexGalleryImages(), TimeSpan.FromMinutes(2));

            //Daily jobs
            recurringJobManager.AddOrUpdate(
                "Update Github projects",
                () => serviceProvider.GetService<IGithubProjectUpdaterService>().Update(),
                Cron.Daily,
                TimeZoneInfo.Local);

            recurringJobManager.AddOrUpdate(
                "Index files on CDS",
                () => serviceProvider.GetService<IGalleryIndexerService>().IndexGalleryImages(),
                Cron.Hourly,
                TimeZoneInfo.Local);
        }
    }
}
