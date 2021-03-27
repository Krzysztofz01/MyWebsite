using System;

#nullable disable

namespace PersonalWebsiteWebApi.Models
{
    public partial class User
    {
        public int Id { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool? Active { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public string LastLoginIp { get; set; }
    }

    public class UserFormDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
