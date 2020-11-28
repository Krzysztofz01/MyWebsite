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

            //HttpClient
            services.AddHttpClient();

            //Controllers
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
