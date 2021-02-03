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
        Task<List<GithubProject>> GetProjects();
        Task PushProjects(IEnumerable<GithubProject> projects);
    }

    public class GithubProjectRepository : IGithubProjectRepository
    {
        private readonly PersonalWebsiteContext context;

        public GithubProjectRepository(
            PersonalWebsiteContext context)
        {
            this.context = context;
        }

        public async Task<List<GithubProject>> GetProjects()
        {
            return await context.GithubProjects.Where(x => x.Display == true).ToListAsync();
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
                    context.GithubProjects.Add(project);
                }
            }

            await context.SaveChangesAsync();
        }
    }
}
