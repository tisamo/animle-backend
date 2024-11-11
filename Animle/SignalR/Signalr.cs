using System.Security.Claims;
using Animle.Classes;
using Animle.Helpers;
using Animle.Models;
using Animle.Services.Auth;
using Microsoft.AspNetCore.SignalR;

namespace Animle.SignalR;

public class SignarlRHub : Hub
{
    private readonly TokenService _tokenService;
    private readonly AnimleDbContext _context;
    public static List<Player> users = new List<Player>();
    private readonly SignalrAnimeService _signalrAnimeService;
    private int selectedAnime = 0;
    private RestartableInterval _restartableInterval;
    private RestartableInterval _tickerInterval;
    private readonly NotificationService _notificationService;


    public SignarlRHub(AnimleDbContext context, TokenService tokenService, SignalrAnimeService signalrAnimeService, NotificationService notificationService )
    {
        _notificationService = notificationService;
        _context = context;
        _tokenService = tokenService;
        _signalrAnimeService = signalrAnimeService;
    }

    public override Task OnConnectedAsync()
    {
        return base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var user = users.FirstOrDefault((u => u.ConnectionId == Context.ConnectionId));
        if (user == null)
        {
            return;
        }

        if (user.opponent != null)
        {
        }


        users.Remove(user);
        if (user.MatchInterval != null)
        {
            user.MatchInterval.Dispose();
        }
        await _context.SaveChangesAsync();

        await base.OnDisconnectedAsync(exception);
    }

    public async Task RegisterPlayer(string token)
    {
        var claimsPrincipal = _tokenService.ValidateToken(token);

        var user = _tokenService.GetUser(_context, claimsPrincipal);

        if (user == null)
        {
        }

        if (users.Any(u => u.Id == user.Id))
        {
            await Clients.Client(Context.ConnectionId).SendAsync("registerError", "You are already in queue");
        }

        var signalRUser = new Player();
        signalRUser.Username = user.Name;
        signalRUser.ConnectionId = Context.ConnectionId;
        signalRUser.IsPlaying = false;
        signalRUser.PlayerWaitingForMatch = true;
        signalRUser.Rating = user.Rating;
        users.Add(signalRUser);
    }

    public async Task FindOpponent()
    {
        var user = users.FirstOrDefault((u => u.ConnectionId == Context.ConnectionId));

        if (user == null)
        {
            await Clients.Client(Context.ConnectionId).SendAsync("opponentNotFound", "opponent not found");
            return;
        };

        var opponent = users
        .Where(u => u.PlayerWaitingForMatch && u.ConnectionId != Context.ConnectionId)
        .OrderBy(u => u.WaitingSince) 
        .FirstOrDefault();

        if (opponent == null)
        {
            await Clients.Client(Context.ConnectionId).SendAsync("opponentNotFound", "opponent not found");
            return;
        }

        user.PlayerWaitingForMatch = false;
        opponent.PlayerWaitingForMatch = false;
        user.opponent = opponent;
        opponent.opponent = user;
        List<AnimeFilter> anim = _signalrAnimeService.GetList().Select(x => new AnimeFilter
        {
            Id = x.Id,
            Title = x.Title,
            Thumbnail = x.Thumbnail,
            Description = x.Description,
            Image = x.Image,
            properties = x.properties,
            EmojiDescription = x.EmojiDescription,
            MyanimeListId = x.MyanimeListId,
        }).Take(3).ToList();

        var rnd = new Random();
        anim.ForEach((a) =>
        {
            var gameType = rnd.Next(0, 4);
            a.Type = UtilityService.GetTypeByNumber(gameType);
        });
        user.animes = anim;
        var counter = 0;
        _tickerInterval = new RestartableInterval(async () => {
            counter++;
            await _notificationService.SendMessageToTheUser(user.ConnectionId, "tick", counter);
            await _notificationService.SendMessageToTheUser(user.opponent.ConnectionId, "tick", counter);

        }, 1000);
        user.MatchInterval = new RestartableInterval(async () => {
            counter = 0;
            if (selectedAnime == anim.Count)
            {
                counter = 0;
                selectedAnime = 0;
                user.MatchInterval.Dispose();
                _tickerInterval.Dispose();
                await GameEnd(user);
                return;
            }
            var date = DateTime.Now;
            var anime = anim.ElementAt(selectedAnime);
            anime.TimeSent = date;
            await _notificationService.SendMessageToTheUser(user.ConnectionId, "nextQuestion", anime);
            await _notificationService.SendMessageToTheUser(user.opponent.ConnectionId, "nextQuestion", anime);
            selectedAnime++;
        }, 15000);

        user.opponent.MatchInterval = user.MatchInterval;
        user.MatchInterval.Start();
        _tickerInterval.Start();

    }


    public async Task Next(int points)
    {
        var user = users.FirstOrDefault((u => u.ConnectionId == Context.ConnectionId));
        if (user != null)
        {
            user.Result += points;
            Console.WriteLine(user.Result);
            if (user.MatchInterval != null)
            {
                user.MatchInterval.Restart();
            }

        };
    }

    private  async Task GameEnd(Player user)
    {
        if (user != null)
        {
            var userResponse = new GameEndResult();
            userResponse.UserResult = (int)user.Result;
            userResponse.OpponentResult = (int)user.opponent.Result;
            var oppoentResoponse = new GameEndResult();
            oppoentResoponse.UserResult = (int)user.opponent.Result;
            oppoentResoponse.OpponentResult = (int)user.Result;
            selectedAnime = 0;
            await _notificationService.SendMessageToTheUser(user.ConnectionId, "gameResult", userResponse);
            await _notificationService.SendMessageToTheUser(user.opponent.ConnectionId, "gameResult", oppoentResoponse);
            users.Remove(user);
            users.Remove(user.opponent);
        };
    }
}