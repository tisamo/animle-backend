using Microsoft.AspNetCore.Mvc;
using NHibernate;
using Animle.services;
using System.Text.Json;
using Animle.Models;
using Microsoft.AspNetCore.RateLimiting;
using NHibernate.Criterion;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System;
using MySqlX.XDevAPI;
using NHibernate.Linq;
using Animle.interfaces;
using NHibernate.Util;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace Animle.Controllers
{

    [Route("anime")]
    [ApiController]
    public class AnimController : ControllerBase
    {

        private readonly ILogger<AnimController> _logger;

        private readonly TokenService _tokenService;

        private readonly AnimleDbContext _animleConect;

        private readonly RequestCacheManager _cacheManager;


        public AnimController(ILogger<AnimController> logger, TokenService tokenService, RequestCacheManager cacheManager, AnimleDbContext animleDbContext)
        {
            _tokenService = tokenService;
            _cacheManager = cacheManager;
            _logger = logger;
            _animleConect = animleDbContext;
        }



        [HttpGet]
        [EnableRateLimiting("fixed")]
        [Route("daily")]
        public async Task<IActionResult> Daily()
        {
            SimpleResponse simpleResponse = new();
            string token = HttpContext.Request.Headers["Authorization"];
            if (token == null)
            {
                simpleResponse.Response = "Login required";
                return Unauthorized(Response);
            }
            var claims = _tokenService.ValidateToken(token);

            if (claims == null)
            {
                simpleResponse.Response = "Token invalid";
                return Unauthorized(Response);
            }
            ContestGame dailyAnimes = _cacheManager.GetCachedItem<ContestGame>("daily");

            var userNameClaim = claims.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);


            User user = _animleConect.Users.Include(u => u.GameContests).FirstOrDefault(u => u.Name == userNameClaim.Value);
          
            user.GameContests.ForEach((u) =>
            {
                Console.WriteLine(u.Id);
            });
            if (user.GameContests.Any(u => u.gameGuid == dailyAnimes.Id))
            {
                simpleResponse.Response = "You have already played this game";

                return BadRequest(simpleResponse);
            }

            Console.WriteLine(user.GameContests.Count);


            if (dailyAnimes == null)
            {
                Random rnd = new Random();
                dailyAnimes = new ContestGame();
                dailyAnimes.Anime = new List<AnimeWithEmoji>(_animleConect.AnimeWithEmoji.OrderBy((item) => rnd.Next()).Take(15));
                dailyAnimes.Anime.ForEach((a) =>
                {
                    int gameType = rnd.Next(0, 3);
                    AnimeWithEmoji animeWithEmoji = a;
                    animeWithEmoji.Type = UtilityService.GetTypeByNumber(gameType);
                    dailyAnimes.Anime.Add(animeWithEmoji);
                });
                _cacheManager.SetCacheItem("daily", dailyAnimes, TimeSpan.FromDays(1));
            }

            var data = UtilityService.Serialize(dailyAnimes);
            var bytes = Encoding.UTF8.GetBytes(data);
            return Ok(Convert.ToBase64String(bytes));
        }

        [HttpPost]
        [EnableRateLimiting("fixed")]
        [Route("daily")]
        public async Task<IActionResult> DailyResult([FromBody] DailyGameResult gameResult )
        {
            SimpleResponse simpleResponse = new();
            string token = HttpContext.Request.Headers["Authorization"];
            if (token == null)
            {
                simpleResponse.Response = "Login required";
                return Unauthorized(Response);
            }
            var claims = _tokenService.ValidateToken(token);

            if (claims == null)
            {
                simpleResponse.Response = "Token invalid";
                return Unauthorized(Response);
            }

            User user = _tokenService.GetUser(_animleConect, claims);

            if (user == null)
            {
                simpleResponse.Response = "User not found";
                return NotFound(Response);
            }
            ContestGame dailyAnimes = _cacheManager.GetCachedItem<ContestGame>("daily");


            if (!user.GameContests.Any(u => u.gameGuid == dailyAnimes.Id))
            {
                GameContest game = new GameContest();
                game.gameGuid = new Guid(gameResult.GameId);
                game.Points = gameResult.Result;
                game.Type = "daily";
                game.TimePlayed = DateTime.Now;
                game.User = user;
                user.GameContests.Add(game);
                await _animleConect.GameContests.AddAsync(game);
                await _animleConect.SaveChangesAsync();

                simpleResponse.Response = "Game saved";

                return Ok(simpleResponse);
            }
            simpleResponse.Response = "You have Already Played this game!";

            return BadRequest(simpleResponse.Response);



        }


        [HttpGet]
        [EnableRateLimiting("fixed")]
        [Route("filter")]
        public async Task<IActionResult> SearchAnime([FromQuery] string q)
        {



            List<AnimeFilter> filteredList = _animleConect.AnimeWithEmoji.Where(x => x.JapaneseTitle.ToLower().Contains(q) || x.Title.ToLower().Contains(q))
               .Select(x => new AnimeFilter
               {

                   Id = x.Id,
                   Title = x.Title,
                   Thumbnail = x.Thumbnail,
                   MyanimeListId = x.MyanimeListId,
                   JapaneseTitle = x.JapaneseTitle,
               })
              .Take(4).ToList();
            return Ok(filteredList);





        }


        [HttpGet]
        [EnableRateLimiting("fixed")]
        [Route("Random")]
        public IActionResult Random()
        {
            var sentAnimes = new List<AnimeFilter>();
            var cachedAnimes = _cacheManager.GetCachedItem<List<AnimeFilter>>("random");
            Random rnd = new Random();
            if (cachedAnimes == null)
            {

                List<AnimeFilter> anim = _animleConect.AnimeWithEmoji.Select(x => new AnimeFilter
                {
                    Id = x.Id,
                    Title = x.Title,
                    Thumbnail = x.Thumbnail,
                    Description = x.Description,
                    Image = x.Image,
                    properties = x.properties,
                    EmojiDescription = x.EmojiDescription,
                    MyanimeListId = x.MyanimeListId,
                }).ToList();
                _cacheManager.SetCacheItem("random", anim, TimeSpan.FromDays(31));
                for (int i = 0; i < 10; i++)
                {
                    int random = rnd.Next(0, anim.Count - 1);
                    int gameType = rnd.Next(0, 3);
                    AnimeFilter animeWithEmoji = anim[random];
                    animeWithEmoji.Type = UtilityService.GetTypeByNumber(gameType);
                    sentAnimes.Add(animeWithEmoji);
                }
                return Ok(sentAnimes);

            }
            else
            {
                for (int i = 0; i < 10; i++)
                {
                    int random = rnd.Next(0, cachedAnimes.Count - 1);
                    int gameType = rnd.Next(0, 3);
                    AnimeFilter animeWithEmoji = cachedAnimes[random];
                    animeWithEmoji.Type = UtilityService.GetTypeByNumber(gameType);
                    sentAnimes.Add(animeWithEmoji);
                }
                return Ok(sentAnimes);
            }

        }

        [HttpGet]
        [EnableRateLimiting("fixed")]
        [Route("tournament")]
        public async Task<IActionResult> GetTournament()
        {


            return Ok("kek");








        }
    }
}


