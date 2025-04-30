using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services;

namespace Services.Implementations
{
    //todo: Add the UserSessionService class to manage user sessions
    //public class UserSessionService : IUserSessionService
    //{
    //    private readonly ApplicationDbContext _context;
    //    private readonly IHttpContextAccessor _httpContextAccessor;

    //    public UserSessionService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    //    {
    //        _context = context;
    //        _httpContextAccessor = httpContextAccessor;
    //    }

    //    public async Task<List<UserSession>> GetActiveSessionsAsync(string userId)
    //    {
    //        return await _context.UserSessions
    //            .Where(s => s.UserId == userId && s.IsActive)
    //            .OrderByDescending(s => s.LoginTime)
    //            .ToListAsync();
    //    }

    //    public async Task TerminateSessionAsync(string userId, string sessionId)
    //    {
    //        var session = await _context.UserSessions
    //            .FirstOrDefaultAsync(s => s.UserId == userId && s.SessionId == sessionId);

    //        if (session != null)
    //        {
    //            session.IsActive = false;
    //            session.LogoutTime = DateTime.UtcNow;
    //            await _context.SaveChangesAsync();
    //        }
    //    }
    //}
}
