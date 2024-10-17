using Animle.Classes;
using Animle.Models;
using Animle.Services.Auth;
using Microsoft.EntityFrameworkCore;

namespace Animle.Services;

public interface IUserService
{
    Task<User> CreateUserAsync(User user);
    Task<User?> AuthenticateUserAsync(LoginInfos loginInfos);
    Task<User?> GetUserByIdAsync(int userId);
    Task FindUnauthenticatedDailyGamesAndPairItToTheUser(User user, string fingerprint);
    Task<List<GameContest>> GetDailyLeaderBoardAsync(string type);
}

public class UserService : IUserService
{
    private readonly AnimleDbContext _context;

    public UserService(AnimleDbContext context)
    {
        _context = context;
    }

    public async Task<User> CreateUserAsync(User user)
    {
        user.Password = PasswordManager.HashPassword(user.Password);
        user.Rating = 1000;
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User?> AuthenticateUserAsync(LoginInfos loginInfos)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Name == loginInfos.Name);
        if (user != null && PasswordManager.VerifyPassword(loginInfos.Password, user.Password))
        {
            return user;
        }

        return null;
    }

    public async Task FindUnauthenticatedDailyGamesAndPairItToTheUser(User user, string fingerprint)
    {
        var anonymousDailyGames = await _context.GameContests
        .Where(gc => gc.Fingerprint == fingerprint && (gc.UserId == null || gc.UserId == 0)) 
        .ToListAsync();

        var anonymousGuessGames = await _context.UserGuessGames
        .Where(gc => gc.Fingerprint == fingerprint && (gc.UserId == null || gc.UserId == 0))
        .ToListAsync();


        anonymousDailyGames.ForEach((ad) =>
        {
            ad.User = user;
            ad.UserId = user.Id;
        });

        anonymousGuessGames.ForEach((ag) =>
        {
            ag.User = user;
            ag.UserId = user.Id;
        });


        await _context.SaveChangesAsync();


    }

    public async Task<User?> GetUserByIdAsync(int userId)
    {
        return await _context.Users.FindAsync(userId);
    }

    public async Task<List<GameContest>> GetDailyLeaderBoardAsync(string type)
    {
        return await _context.GameContests
            .Include(g => g.User)
            .OrderBy(g => g.Result)
            .Take(25)
            .ToListAsync();
    }
}