using Microsoft.AspNetCore.Http;
using PersonalWebsiteWebApi.Models;
using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace PersonalWebsiteWebApi.Services
{
    public interface IImageFileHandlerService
    {
        Task<GalleryImage> GetModel(IFormFile file, string name, string category);
        Task<Bitmap> ResizeImage(IFormFile file);
        Task<Bitmap> GetImage(IFormFile file);
    }

    public class ImageFileHandlerService : IImageFileHandlerService
    {
        public async Task<GalleryImage> GetModel(IFormFile file, string name, string category)
        {
            string extesion = Path.GetExtension(file.FileName).ToLower();
            if(extesion == ".png" || extesion == ".jpg")
            {
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    if (memoryStream.Length > 58000000) return null;

                    string guid = $"{ Guid.NewGuid() }{ extesion }";
                    return new GalleryImage()
                    {
                        Name = name,
                        Category = category,
                        Filename = guid
                    };
                }
            }
            return null;
        }

        public async Task<Bitmap> ResizeImage(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                using (var image = Image.FromStream(memoryStream))
                {
                    int newWidth = 1920;
                    int newHeight = (newWidth * image.Height) / image.Width;
                    return new Bitmap(image, newWidth, newHeight);
                }     
            }
        }

        public async Task<Bitmap> GetImage(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                using (var image = Image.FromStream(memoryStream))
                {
                    return new Bitmap(image);
                }
            }
        }
    }
}
