using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace LibraryAPI.Infrastructure.Services.GuardService
{
    public interface IGuardService
    {
        Task<bool> ValidateRole(string token, Role role);

        Task<Session> ValidateSession(string token);

        //public string ExtractToken(string requestToken);
    }
}
