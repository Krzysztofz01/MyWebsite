using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PersonalWebsiteWebApi.Models;
using PersonalWebsiteWebApi.Repositories;
using PersonalWebsiteWebApi.Services;

namespace PersonalWebsiteWebApi.Controllers
{
    [Route("/api/github")]
    [ApiController]
    public class GithubProjectsController : ControllerBase
    {
        private readonly IGithubProjectRepository githubProjectRepository;
        private readonly IGithubProjectUpdaterService githubProjectUpdaterService;

        public GithubProjectsController(
            IGithubProjectRepository githubProjectRepository,
            IGithubProjectUpdaterService githubProjectUpdaterService)
        {
            this.githubProjectRepository = githubProjectRepository;
            this.githubProjectUpdaterService = githubProjectUpdaterService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GithubProject>>> GetGithubProjects()
        {
            await githubProjectUpdaterService.Update();
            Console.WriteLine("repo pushed into db");
            return await githubProjectRepository.GetProjects();
        }
    }
}
