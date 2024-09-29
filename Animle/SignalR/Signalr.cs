﻿using System.Security.Claims;
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

    public SignarlRHub(AnimleDbContext context, TokenService tokenService, SignalrAnimeService signalrAnimeService)
    {
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
            await Clients.Client(user.opponent.ConnectionId).SendAsync("opponentDisconnected");
        }


        users.Remove(user);
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
        }

        ;

        var opponent = users.Where(u => u.PlayerWaitingForMatch && u.ConnectionId != Context.ConnectionId)
            .OrderBy((x => new Guid())).FirstOrDefault();

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
        }).Take(10).ToList();

        var rnd = new Random();
        anim.ForEach((a) =>
        {
            var random = rnd.Next(0, anim.Count - 1);
            var gameType = rnd.Next(0, 3);
            a.Description = a.Description;
            a.Type = UtilityService.GetTypeByNumber(gameType);
        });

        await Clients.Client(user.ConnectionId).SendAsync("opponentFound", anim);
        await Clients.Client(opponent.ConnectionId).SendAsync("opponentFound", anim);
    }


    public async Task Next()
    {
        var user = users.FirstOrDefault((u => u.ConnectionId == Context.ConnectionId));
        if (user != null)
        {
            await Clients.Client(user.ConnectionId).SendAsync("nextQuestion");
            await Clients.Client(user.opponent.ConnectionId).SendAsync("nextQuestion");
        }

        ;
    }

    public async Task GameEnd(int result)
    {
        var user = users.FirstOrDefault((u => u.ConnectionId == Context.ConnectionId));
        user.Result = result;
        if (user.Result != null && user.opponent.Result != null)
        {
            var userResponse = new GameEndResult();
            userResponse.UserResult = (int)user.Result;
            userResponse.OpponentResult = (int)user.opponent.Result;
            var oppoentResoponse = new GameEndResult();
            oppoentResoponse.UserResult = (int)user.opponent.Result;
            oppoentResoponse.OpponentResult = (int)user.Result;

            await Clients.Client(user.ConnectionId).SendAsync("gameResult", userResponse);
            await Clients.Client(user.opponent.ConnectionId).SendAsync("gameResult", oppoentResoponse);
            users.Remove(user);
            users.Remove(user.opponent);
        }

        ;
    }
}