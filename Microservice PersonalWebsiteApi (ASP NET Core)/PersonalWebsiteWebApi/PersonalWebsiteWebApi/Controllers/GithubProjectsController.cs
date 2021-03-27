using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using PersonalWebsiteWebApi.Models;
using PersonalWebsiteWebApi.Repositories;
using PersonalWebsiteWebApi.Services;
using System.Drawing.Imaging;
using Microsoft.AspNetCore.Authorization;

namespace PersonalWebsiteWebApi.Controllers
{
    [Route("/api/github")]
    [ApiController]
    public class GithubProjectsController : ControllerBase
    {
        private readonly IGithubProjectRepository githubProjectRepository;
        private readonly IGalleryImageRepository galleryImageRepository;
        private readonly IImageFileHandlerService imageFileHandlerService;
        private readonly IWebHostEnvironment webHostEnvironment;

        public GithubProjectsController(
            IGithubProjectRepository githubProjectRepository,
            IGalleryImageRepository galleryImageRepository,
            IImageFileHandlerService imageFileHandlerService,
            IWebHostEnvironment webHostEnvironment)
        {
            this.githubProjectRepository = githubProjectRepository;
            this.galleryImageRepository = galleryImageRepository;
            this.imageFileHandlerService = imageFileHandlerService;
            this.webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GithubProject>>> GetGithubProjects()
        {
            return await githubProjectRepository.GetProjects();
        }

        [HttpPost("image")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> SetProjectImage([FromForm]ProjectImageDto form)
        {
            if (form.File == null || form.File.Length == 0) return BadRequest();

            var project = await githubProjectRepository.GetProjectById(form.Id);
            if (project == null) return BadRequest();

            var imageModel = await imageFileHandlerService.GetModel(form.File, project.Name, "Repo");
            if (imageModel == null) return BadRequest();

            var image = await imageFileHandlerService.GetImage(form.File);
            if (image == null) return BadRequest();

            using (var fileStream = new FileStream(Path.Combine(webHostEnvironment.ContentRootPath, Path.Combine("StaticImages", imageModel.Filename)), FileMode.Create, FileAccess.Write))
            {
                image.Save(fileStream, (imageModel.Filename.EndsWith(".jpg") ? ImageFormat.Jpeg : ImageFormat.Png));
                await galleryImageRepository.AddImage(imageModel);
                var imageUrl = await galleryImageRepository.GetRepoImageUrl(project.Name);
                if (imageUrl == null) return Conflict();
                await githubProjectRepository.SetProjectImage(form.Id, imageUrl);

                return Ok($"{project.Name}: {imageUrl}");
            }
        }

        [HttpPost("display")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> ChangeDisplay(ProjectDisplayDto form)
        {
            if (await githubProjectRepository.ChangeDisplay(form.Id, form.Display)) return Ok();
            return BadRequest();
        }
    }
}
