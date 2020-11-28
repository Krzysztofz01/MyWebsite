using System;

#nullable disable

namespace PersonalWebsiteWebApi.Models
{
    public partial class GalleryImage
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string Category { get; set; }
        public bool? Display { get; set; }
        public DateTime? CreateDate { get; set; }
    }

    public class GalleryImageDto
    {
        public string ImageUrl { get; set; }
        public string Category { get; set; }
    }
}
