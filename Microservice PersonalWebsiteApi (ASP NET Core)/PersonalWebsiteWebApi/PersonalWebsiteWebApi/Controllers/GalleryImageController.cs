using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using PersonalWebsiteWebApi.Models;
using PersonalWebsiteWebApi.Repositories;
using PersonalWebsiteWebApi.Services;

namespace PersonalWebsiteWebApi.Controllers
{
    [Route("api/gallery")]
    [ApiController]
    public class GalleryImageController : ControllerBase
    {
        private readonly IGalleryImageRepository galleryImageRepository;
        private readonly IImageFileHandlerService imageFileHandlerService;
        private readonly IWebHostEnvironment webHostEnvironment; 

        public GalleryImageController(
            IGalleryImageRepository galleryImageRepository,
            IImageFileHandlerService imageFileHandlerService,
            IWebHostEnvironment webHostEnvironment)
        {
            this.galleryImageRepository = galleryImageRepository;
            this.imageFileHandlerService = imageFileHandlerService;
            this.webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GalleryImageDto>>> GetAllImages()
        {
            return Ok(await galleryImageRepository.GetAllImages());
        }

        [HttpPost("upload")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UploadImage([FromForm]GalleryImageUpload giu)
        {
            if (giu.File == null || giu.File.Length == 0) return BadRequest();

            var imageModel = await imageFileHandlerService.GetModel(giu.File, giu.Name, giu.Category);
            if (imageModel == null) return BadRequest();

            var image = await imageFileHandlerService.ResizeImage(giu.File);
            if (image == null) return BadRequest();

            using (var fileStream = new FileStream(Path.Combine(webHostEnvironment.ContentRootPath, Path.Combine("StaticImages", imageModel.Filename)), FileMode.Create, FileAccess.Write))
            {
                image.Save(fileStream, (imageModel.Filename.EndsWith(".jpg") ? ImageFormat.Jpeg : ImageFormat.Png));
                await galleryImageRepository.AddImage(imageModel);
            }
            return Ok();
        }

        [HttpPost("display")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> ChangeDisplay(GalleryDisplayDto form)
        {
            if (await galleryImageRepository.ChangeDisplay(form.Id, form.Display)) return Ok();
            return BadRequest();
        }

        [HttpGet("display")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> ShowDisplay()
        {
            return Ok(await galleryImageRepository.ShowDisplay());
        }
    }
}
