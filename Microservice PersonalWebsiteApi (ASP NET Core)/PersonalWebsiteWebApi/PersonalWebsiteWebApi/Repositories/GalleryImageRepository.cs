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

            var images = await context.GalleryImages.Where(x => x.Display == true).ToListAsync();
            foreach (var image in images) imagesDto.Add(new GalleryImageDto() { ImageUrl = image.ImageUrl, Category = image.Category });

            return imagesDto;
        }

        public async Task PushImages(IEnumerable<GalleryImage> images)
        {
            context.GalleryImages.AddRange(images);
            await context.SaveChangesAsync();
        }
    }
}
