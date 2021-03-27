using Microsoft.EntityFrameworkCore;
using PersonalWebsiteWebApi.DatabaseContext;
using PersonalWebsiteWebApi.Models;
using PersonalWebsiteWebApi.Services;
using System;
using System.Threading.Tasks;

namespace PersonalWebsiteWebApi.Repositories
{
    public interface IUserRepository
    {
        Task<string> Login(string email, string password, string ipAddress);
        Task<bool> Register(UserFormDto user, string ipAddress);
    }

    public class UserRepository : IUserRepository
    {
        private readonly PersonalWebsiteContext context;
        private readonly IAuthenticationService authenticationService;

        public UserRepository(
            PersonalWebsiteContext context,
            IAuthenticationService authenticationService)
        {
            this.context = context;
            this.authenticationService = authenticationService;
        }

        public async Task<string> Login(string email, string password, string ipAddress)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Email == email);
            if(user != null)
            {
                if(BCrypt.Net.BCrypt.Verify(password, user.Password))
                {
                    if(user.Active == true)
                    {
                        user.LastLoginDate = DateTime.Now;
                        user.LastLoginIp = ipAddress ?? "";
                        context.Entry(user).State = EntityState.Modified;

                        await context.SaveChangesAsync();
                        return authenticationService.GenerateToken(user);
                    }
                }
            }
            return null;
        }

        public async Task<bool> Register(UserFormDto userForm, string ipAddress)
        {
            var existingUser = await context.Users.FirstOrDefaultAsync(x => x.Email == userForm.Email);
            if(existingUser == null)
            {
                var user = new User()
                {
                    Email = userForm.Email,
                    Active = false,
                    LastLoginDate = DateTime.Now,
                    LastLoginIp = ipAddress ?? ""
                };

                var salt = BCrypt.Net.BCrypt.GenerateSalt(5);
                user.Password = BCrypt.Net.BCrypt.HashPassword(userForm.Password, salt);

                context.Users.Add(user);
                if(await context.SaveChangesAsync() > 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
