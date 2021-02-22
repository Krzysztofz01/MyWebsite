using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PersonalWebsiteWebApi.Models;
using PersonalWebsiteWebApi.Repositories;
using PersonalWebsiteWebApi.Services;
using PersonalWebsiteWebApi.Settings;

namespace PersonalWebsiteWebApi.Controllers
{
    [Route("api/gallery")]
    [ApiController]
    public class GalleryImageController : ControllerBase
    {
        private readonly IGalleryImageRepository galleryImageRepository;
        private readonly IImageFileHandlerService imageFileHandlerService;
        private readonly IWebHostEnvironment webHostEnvironment; 
        private readonly AuthSettings authSettings;

        public GalleryImageController(
            IGalleryImageRepository galleryImageRepository,
            IImageFileHandlerService imageFileHandlerService,
            IOptions<AuthSettings> authSettings,
            IWebHostEnvironment webHostEnvironment)
        {
            this.galleryImageRepository = galleryImageRepository;
            this.imageFileHandlerService = imageFileHandlerService;
            this.authSettings = authSettings.Value;
            this.webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GalleryImageDto>>> GetAllImages()
        {
            return Ok(await galleryImageRepository.GetAllImages());
        }

        
        [HttpPost]
        public async Task<ActionResult> UploadImage([FromForm]GalleryImageUpload giu)
        {
            if(giu.Token == authSettings.Token)
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
                return Ok(Path.Combine(webHostEnvironment.ContentRootPath, imageModel.Filename).ToString());
            }
            return BadRequest();
        }
    }
}
