using Azure.Core;
using LibraryAPI.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LibraryAPI.Infrastructure.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;
        public UserService(DataContext datacontext, IConfiguration configuration)
        {
            _dataContext = datacontext;
            _configuration = configuration;
        }
        public async Task<User> AddUser(User user)
        {
            _dataContext.Users.Add(user);
            await _dataContext.SaveChangesAsync();
            return user;
        }
        public async Task<User?> GetOnlyUser(string username, string password)
        {
            var user = await _dataContext.Users.Where(x => x.UserName == username).FirstOrDefaultAsync();
            if (user is null || !BCrypt.Net.BCrypt.Verify(password, user.HashPassword))
            {
                return null;
            }
            var sessions = await _dataContext.Sessions.Where(x => x.Status == SessionStatus.Open && x.UserId == user.UserId).ToListAsync();
            foreach (var x in sessions)
            {
                x.Status = SessionStatus.Closed;
                x.DeletedAt = DateTime.UtcNow;
            }
            string token = CreateToken();
            Session session = new Session { Token = token, UserId = user.UserId, Status = SessionStatus.Open, User = user };
            _dataContext.Sessions.Add(session);
            await _dataContext.SaveChangesAsync();
            return user;
        }

        public async Task<User?> GetUserByToken(string token)
        {
            var session = await _dataContext.Sessions.Where(x => x.Token == token && x.Status != SessionStatus.Closed && x.DeletedAt == null).FirstOrDefaultAsync();
            if (session is null)
            {
                return null;
            }
            return session.User;
        }

        public async Task<bool> Logout(Session session)
        {
            session.Status = SessionStatus.Closed;
            session.DeletedAt = DateTime.UtcNow;
            await _dataContext.SaveChangesAsync();
            return true;
        }

        private string CreateToken()
        {
            int length = 25;
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var randomString = new string(Enumerable.Repeat(chars, length)
                                                    .Select(s => s[random.Next(s.Length)]).ToArray());
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, randomString)
            };


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
               _configuration.GetSection("AppSettings:Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds
                );

            var _token = new JwtSecurityTokenHandler().WriteToken(token);

            return _token;
        }

    }
}

