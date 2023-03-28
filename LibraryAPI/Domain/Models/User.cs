using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace LibraryAPI.Domain.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string HashPassword { get; set; } = string.Empty;
        public List<Book>? Books { get; set; } = new();
        public Role Role { get; set; } = Role.User;

        public List<Session> Sessions { get; set; }
    }

    public enum Role
    {
        Admin,
        User
    }
}
