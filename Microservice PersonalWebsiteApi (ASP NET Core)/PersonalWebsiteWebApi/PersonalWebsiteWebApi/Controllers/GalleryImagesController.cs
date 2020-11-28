using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PersonalWebsiteWebApi.Models;
using PersonalWebsiteWebApi.Repositories;

namespace PersonalWebsiteWebApi.Controllers
{
    [Route("/api/gallery")]
    [ApiController]
    public class GalleryImagesController : ControllerBase
    {
        private readonly IGalleryImageRepository galleryImageRepository;

        public GalleryImagesController(
            IGalleryImageRepository galleryImageRepository)
        {
            this.galleryImageRepository = galleryImageRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GalleryImageDto>>> GetGithubProjects()
        {
            return await galleryImageRepository.GetImages();
        }
    }
}
