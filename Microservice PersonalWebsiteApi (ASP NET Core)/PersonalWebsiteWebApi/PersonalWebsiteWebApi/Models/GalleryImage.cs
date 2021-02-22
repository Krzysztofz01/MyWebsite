using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace PersonalWebsiteWebApi.Models
{
    public partial class GalleryImage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Filename { get; set; }
        public bool? Display { get; set; }
        public DateTime? CreateDate { get; set; }
    }

    public class GalleryImageDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Url { get; set; }
    }

    public class GalleryImageUpload
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public IFormFile File { get; set; }

        [Required]
        public string Token { get; set; }
    }
}
