
namespace LibraryAPI.Infrastructure.Services.UserService
{
    public interface IUserService
    {
        Task<User?> GetOnlyUser(string username, string password);
        Task<User> AddUser(User user);

        Task<bool> Logout(Session session);
    }
}
