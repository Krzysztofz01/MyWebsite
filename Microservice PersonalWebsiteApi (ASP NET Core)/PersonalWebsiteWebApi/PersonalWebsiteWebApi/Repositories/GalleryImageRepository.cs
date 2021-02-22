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
        Task<bool> AddImage(GalleryImage image);
        Task<bool> DeleteImage(int id);
        Task<GalleryImage> GetImage(int id);
        Task<IEnumerable<GalleryImageDto>> GetAllImages();
    }

    public class GalleryImageRepository : IGalleryImageRepository
    {
        private readonly string StaticFilesUrl = "cds/images/";
        private readonly PersonalWebsiteContext context;

        public GalleryImageRepository(
            PersonalWebsiteContext context)
        {
            this.context = context;
        }

        public async Task<bool> AddImage(GalleryImage image)
        {
            context.Add(image);
            if(await context.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteImage(int id)
        {
            var image = context.GalleryImages.FirstOrDefault(x => x.Id == id);
            if(image != null)
            {
                context.Entry(image).State = EntityState.Deleted;
                if (await context.SaveChangesAsync() > 0) return true;
            }
            return false;
        }

        public async Task<IEnumerable<GalleryImageDto>> GetAllImages()
        {
            var images = await context.GalleryImages.Where(x => x.Display == true).ToListAsync();
            var dtoContainer = new List<GalleryImageDto>();

            foreach(var image in images)
            {
                dtoContainer.Add(new GalleryImageDto()
                {
                    Id = image.Id,
                    Name = image.Name,
                    Category = image.Category,
                    Url = $"{StaticFilesUrl}{image.Filename}"
                });
            }
            return dtoContainer; 
        }

        public async Task<GalleryImage> GetImage(int id)
        {
            return await context.GalleryImages.FirstOrDefaultAsync(x => x.Id == id && x.Display == true);
        }
    }
}
