using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PersonalWebsite.Models;
using PersonalWebsite.Repositories;

namespace PersonalWebsite.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGithubRepository githubRepository;
        //for developement

        public HomeController(
            IGithubRepository githubRepository)
        {
            this.githubRepository = githubRepository;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                Console.WriteLine(JsonConvert.SerializeObject(await githubRepository.GetGithubRepositories()));
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message + " " + e.StackTrace);
            }
                return View();
        }
    }
}
