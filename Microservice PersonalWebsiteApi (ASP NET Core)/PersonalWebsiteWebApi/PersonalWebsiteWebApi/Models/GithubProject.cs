using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace PersonalWebsiteWebApi.Models
{
    public partial class GithubProject
    {
        [JsonIgnore]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "html_url")]
        public string HtmlUrl { get; set; }

        public string ImageUrl { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "language")]
        public string Language { get; set; }

        [JsonProperty(PropertyName = "created_at")]
        public DateTime ProjectCreated { get; set; }

        [JsonProperty(PropertyName = "updated_at")]
        public DateTime ProjectUpdated { get; set; }

        public bool? Display { get; set; }

        public DateTime? CreateDate { get; set; }
    }

    public class ProjectImageDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public IFormFile File { get; set; }
    }

    public class ProjectDisplayDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public bool Display { get; set; }
    }
}