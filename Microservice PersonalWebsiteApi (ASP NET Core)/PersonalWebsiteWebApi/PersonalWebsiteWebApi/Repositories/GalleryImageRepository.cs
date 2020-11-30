using Microsoft.EntityFrameworkCore;
using PersonalWebsiteWebApi.DatabaseContext;
using PersonalWebsiteWebApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalWebsiteWebApi.Repositories
{
    public interface IGalleryImageRepository
    {
        Task<List<GalleryImageDto>> GetImages();
        Task PushImages(IEnumerable<GalleryImage> images);
    }

    public class GalleryImageRepository : IGalleryImageRepository
    {
        private readonly PersonalWebsiteContext context;

        public GalleryImageRepository(
            PersonalWebsiteContext context)
        {
            this.context = context;
        }
        public async Task<List<GalleryImageDto>> GetImages()
        {
            var imagesDto = new List<GalleryImageDto>();

            var images = await context.GalleryImages.Where(x => x.Display == true && x.FileExist == true).ToListAsync();
            foreach (var image in images) imagesDto.Add(new GalleryImageDto() { ImageUrl = image.ImageUrl, Category = image.Category });

            return imagesDto;
        }

        public async Task PushImages(IEnumerable<GalleryImage> images)
        {
            var allImages = await context.GalleryImages.ToArrayAsync();
            foreach(var image in allImages)
            {
                image.FileExist = false;
                var currentImage = images.Where(x => x.ImageUrl == image.ImageUrl).FirstOrDefault();
                if(currentImage != null)
                {
                    image.FileExist = true;
                }
                context.Entry(image).State = EntityState.Modified;
            }

            var newImages = images.Where(x => !allImages.Any(z => z.ImageUrl == x.ImageUrl)).ToList();
            context.GalleryImages.AddRange(newImages);

            await context.SaveChangesAsync();
        }
    }
}
