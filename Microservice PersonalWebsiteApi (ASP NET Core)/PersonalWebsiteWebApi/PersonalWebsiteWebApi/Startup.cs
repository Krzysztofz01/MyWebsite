using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using PersonalWebsiteWebApi.DatabaseContext;
using PersonalWebsiteWebApi.Repositories;
using PersonalWebsiteWebApi.Services;
using PersonalWebsiteWebApi.Settings;
using System;
using System.IO;

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
            services.Configure<AuthSettings>(Configuration.GetSection("AuthSettings"));

            //Database
            services.AddDbContext<PersonalWebsiteContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("PersonalWebsite")));

            //Repositories
            services.AddScoped<IGithubProjectRepository, GithubProjectRepository>();
            services.AddScoped<IGalleryImageRepository, GalleryImageRepository>();

            //Services
            services.AddTransient<IGithubProjectUpdaterService, GithubProjectUpdaterService>();
            services.AddTransient<IImageFileHandlerService, ImageFileHandlerService>();

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

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "StaticImages")),
                RequestPath = "/cds/images"
            });

            app.UseRouting();

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
