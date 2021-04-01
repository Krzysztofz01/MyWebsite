using Microsoft.EntityFrameworkCore;
using PersonalWebsiteWebApi.DatabaseContext;
using PersonalWebsiteWebApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalWebsiteWebApi.Repositories
{
    public interface IGithubProjectRepository
    {
        Task<GithubProject> GetProjectById(int id);
        Task<List<GithubProject>> GetProjects();
        Task<bool> ChangeDisplay(int id, bool display);
        Task<IEnumerable<ProjectShowDisplayDto>> ShowDisplay();
        Task PushProjects(IEnumerable<GithubProject> projects);
        Task<bool> SetProjectImage(int id, string imageUrl);
    }

    public class GithubProjectRepository : IGithubProjectRepository
    {
        private readonly PersonalWebsiteContext context;

        public GithubProjectRepository(
            PersonalWebsiteContext context)
        {
            this.context = context;
        }

        public async Task<GithubProject> GetProjectById(int id)
        {
            return await context.GithubProjects.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<GithubProject>> GetProjects()
        {
            return await context.GithubProjects.Where(x => x.Display == true).ToListAsync();
        }

        public async Task<bool> ChangeDisplay(int id, bool display)
        {
            var project = await context.GithubProjects.FirstOrDefaultAsync(x => x.Id == id);
            if (project != null)
            {
                if (project.Display != display)
                {
                    project.Display = display;
                    context.Entry(project).State = EntityState.Modified;

                    if (await context.SaveChangesAsync() <= 0) return false;
                }
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<ProjectShowDisplayDto>> ShowDisplay()
        {
            var projects = await context.GithubProjects.ToListAsync();
            var dtoContainer = new List<ProjectShowDisplayDto>();
            foreach (var project in projects)
            {
                dtoContainer.Add(new ProjectShowDisplayDto()
                {
                    Id = project.Id,
                    Name = project.Name,
                    ImageUrl = project.ImageUrl,
                    Display = project.Display
                });
            }
            return dtoContainer;
        }

        public async Task PushProjects(IEnumerable<GithubProject> projects)
        {
            foreach(var project in projects)
            {
                var storedProject = context.GithubProjects.FirstOrDefault(x => x.Name == project.Name);
                if(storedProject != null)
                {
                    bool itemChanged = false;

                    if(storedProject.Description != project.Description)
                    {
                        storedProject.Description = project.Description;
                        itemChanged = true;
                    }

                    if(storedProject.ProjectUpdated != project.ProjectUpdated)
                    {
                        storedProject.ProjectUpdated = project.ProjectUpdated;
                        itemChanged = true;
                    }
                    
                    if(itemChanged)
                    {
                        context.Entry(storedProject).State = EntityState.Modified;
                    }
                }
                else
                {
                    project.ImageUrl = "default";
                    project.Display = false;
                    context.GithubProjects.Add(project);
                }
            }

            await context.SaveChangesAsync();
        }

        public async Task<bool> SetProjectImage(int id, string imageUrl)
        {
            var project = await context.GithubProjects.FirstOrDefaultAsync(x => x.Id == id);
            project.ImageUrl = imageUrl;

            context.Entry(project).State = EntityState.Modified;
            
            if(await context.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;
        }
    }
}
