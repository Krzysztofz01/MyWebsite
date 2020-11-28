using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PersonalWebsiteWebApi.Models;
using PersonalWebsiteWebApi.Repositories;

namespace PersonalWebsiteWebApi.Controllers
{
    [Route("/api/github")]
    [ApiController]
    public class GithubProjectsController : ControllerBase
    {
        private readonly IGithubProjectRepository githubProjectRepository;

        public GithubProjectsController(
            IGithubProjectRepository githubProjectRepository)
        {
            this.githubProjectRepository = githubProjectRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GithubProject>>> GetGithubProjects()
        {
            return await githubProjectRepository.GetProjects();
        }
    }
}
