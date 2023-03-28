using AutoMapper;
using LibraryAPI.Domain.Models;
using LibraryAPI.Infrastructure.Persistence.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Server.IIS.Core;

namespace LibraryAPI.Infrastructure.Services.GuardService
{
    public class GuardService : IGuardService
    {

        private readonly DataContext _dataContext;

        public GuardService(DataContext datacontext)
        {
            _dataContext = datacontext;
        }


        public async Task<bool> ValidateRole(string token, Role role)
        {
            Session session = await ValidateSession(token);
            if (session.User.Role != role)
            {
                throw new Exception("You are not admin");
            }
            return true;
        }

        public async Task<Session> ValidateSession(string token)
        {
            string cutToken = ExtractToken(token);

            Session session = await _dataContext.Sessions.Include(session => session.User).Include(s => s.User.Books).Where(s => s.Token == cutToken && s.Status == SessionStatus.Open && s.DeletedAt == null).FirstOrDefaultAsync();
            if (session is null)
            {
                throw new Exception("Unauthorazied");
            }
            return session;
        }
        private string ExtractToken(string requestToken)
        {
            if (!requestToken.StartsWith("bearer "))
            {
                throw new NotImplementedException();
            }

            return requestToken.Substring(7);
        }
    }
}
