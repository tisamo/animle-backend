using Animle.Classes;
using Animle.Helpers;
using Animle.Interfaces;
using Animle.Migrations;
using Animle.Models;
using Microsoft.EntityFrameworkCore;
using NHibernate.Linq;
using Org.BouncyCastle.Tls;

namespace Animle.Services
{
    public interface IAnimeService
    {
        Task<DailyChallenge> GetDailyChallengeAsync(User? user, string fingerPrint);
        Task<SimpleResponse> SubmitGameResultAsync(string type, User user, DailyGameResult gameResult);

        Task<SimpleResponse> SubmitGameResultAnonymously(string type, DailyGameResult gameResult);


        Task<SimpleResponse> SubmitLoggedInUserProgress(User user, GuessGameProgress progress);

        Task<SimpleResponse> SubmitUnAuthenticatedUserProgress( GuessGameProgress progress);


        Task<List<AnimeFilter>> FilterAnimeAsync(string query);
        List<AnimeFilter> GetRandomAnimes();
        dynamic GetEmojiQuiz(User user, string fingerPrint);
        dynamic GetEmojiQuizUnAuthenticated(string fingerPrint);

    }

    public class AnimeService : IAnimeService
    {
        private readonly AnimleDbContext _dbContext;
        private readonly IRequestCacheManager _cacheManager;

        public AnimeService(AnimleDbContext dbContext, IRequestCacheManager cacheManager)
        {
            _dbContext = dbContext;
            _cacheManager = cacheManager;
        }

        public async Task<DailyChallenge> GetDailyChallengeAsync(User? user, string fingerPrint)
        {
            var dailyChallenge = _cacheManager.GetCachedItem<DailyChallenge>("daily");

            if(user == null)
            {
                if (_dbContext.GameContests.Any(g => g.Fingerprint == fingerPrint && g.DailyChallengeId == dailyChallenge.Id))
                {
                    return null;
                }
            }
            else
            {

                if (_dbContext.GameContests.Any(g => (g.UserId == user.Id || g.Fingerprint == fingerPrint) && g.DailyChallengeId == dailyChallenge.Id))
                {
                    return null;
                }
            }



            return dailyChallenge;
        }

        public async Task<SimpleResponse> SubmitLoggedInUserProgress(User user, GuessGameProgress progress)
        {
           
               var userGame = _cacheManager.GetCachedItem<GuessGame>("dailyGuess");

               UserGuessGame? gameInProgress  = _dbContext.UserGuessGames.FirstOrDefault(u => (u.UserId == user.Id || u.Fingerprint == progress.Fingerprint) && u.GuessGameId == userGame.Id);


            if (gameInProgress != null)
            {
                if (gameInProgress.Attempts >= 5 ||gameInProgress.Result > 0) {
                    return new SimpleResponse { IsSuccess = false, Response = "You already Played this game!" };
                }
                else
                {
                    gameInProgress.Result = progress.Result;
                    gameInProgress.Attempts = progress.Attempts;
                    _dbContext.UserGuessGames.Update(gameInProgress);
                    await _dbContext.SaveChangesAsync();
                    return new SimpleResponse { IsSuccess = true, Response = "Game state updated!" };
                }  
            }
                var guessGame = new UserGuessGame
                {
                    User = user,
                    UserId = user.Id,
                    Created = DateTime.Now,
                    Fingerprint =  progress.Fingerprint,
                    Result = progress.Result,
                    GuessGame = userGame,
                    GuessGameId = userGame.Id
                };
            _dbContext.UserGuessGames.Add(guessGame);
            await _dbContext.SaveChangesAsync();
                return new SimpleResponse { IsSuccess = true, Response = "Game saved" };
            
        }

        public async Task<SimpleResponse> SubmitUnAuthenticatedUserProgress(GuessGameProgress progress)
        {

            var userGame = _cacheManager.GetCachedItem<GuessGame>("dailyGuess");

            UserGuessGame? gameInProgress = _dbContext.UserGuessGames.FirstOrDefault(u =>(u.Fingerprint == progress.Fingerprint) && userGame.Id == u.GuessGameId);


            if (gameInProgress != null)
            {
                if (gameInProgress.Attempts >= 5 || gameInProgress.Result > 0)
              

                    {
                        return new SimpleResponse { IsSuccess = false, Response = "You already Played this game!" };
                }
                else
                {
                    gameInProgress.Result = progress.Result;
                    Console.WriteLine(progress.Attempts);
                    gameInProgress.Attempts = progress.Attempts;
                     _dbContext.UserGuessGames.Update(gameInProgress);
                    await _dbContext.SaveChangesAsync();
                    return new SimpleResponse { IsSuccess = true, Response = "Game state updated!" };
                }
            }

            var guessGame = new UserGuessGame
            {
                User = null, 
                UserId = null,
                Created = DateTime.Now,
                Fingerprint = progress.Fingerprint,
                Result = progress.Result,
                GuessGame = userGame,
                GuessGameId = userGame.Id
            };
            _dbContext.UserGuessGames.Add(guessGame);
            await _dbContext.SaveChangesAsync();
            return new SimpleResponse { IsSuccess = true, Response = "Game saved" };

        }
        public async Task<SimpleResponse> SubmitGameResultAsync(string type, User user, DailyGameResult gameResult)
        {
            if (type == "daily")
            {
                var dailyChallenge = _cacheManager.GetCachedItem<DailyChallenge>("daily");


                if (_dbContext.GameContests.Include(u => u.User)
                    .Any(g => g.UserId == user.Id && g.DailyChallengeId == dailyChallenge.Id))
                {
                    return new SimpleResponse { IsSuccess = false, Response = "You have already played this game." };
                }

                var gameContest = new GameContest
                {
                    User = user,
                    UserId = user.Id,
                    TimePlayed = DateTime.Now,
                    Fingerprint = gameResult.fingerprint,
                    Result = gameResult.Result,
                    Challenge = dailyChallenge,
                    DailyChallengeId = dailyChallenge.Id
                };
                _dbContext.ChangeTracker.Clear();
                _dbContext.GameContests.Attach(gameContest);
                await _dbContext.SaveChangesAsync();

                return new SimpleResponse { IsSuccess = true, Response = "Game saved" };
            }

            return new SimpleResponse { IsSuccess = false, Response = "Invalid game type" };
        }


        public async Task<SimpleResponse> SubmitGameResultAnonymously(string type, DailyGameResult gameResult)
        {
            if (type == "daily")
            {
                var dailyChallenge = _cacheManager.GetCachedItem<DailyChallenge>("daily");


                if (_dbContext.GameContests.Include(u => u.User)
                    .Any(g => g.Fingerprint == gameResult.fingerprint && g.DailyChallengeId == dailyChallenge.Id))
                {
                    return new SimpleResponse { IsSuccess = false, Response = "You have already played this game." };
                }

                var gameContest = new GameContest
                {
                    User = null,
                    UserId = null,
                    Fingerprint = gameResult.fingerprint,
                    TimePlayed = DateTime.Now,
                    Result = gameResult.Result,
                    Challenge = dailyChallenge,
                    DailyChallengeId = dailyChallenge.Id
                };
                _dbContext.ChangeTracker.Clear();
                _dbContext.GameContests.Attach(gameContest);
                await _dbContext.SaveChangesAsync();

                return new SimpleResponse { IsSuccess = true, Response = "Game saved" };
            }

            return new SimpleResponse { IsSuccess = false, Response = "Invalid game type" };
        }

        public async Task<List<AnimeFilter>> FilterAnimeAsync(string query)
        {
            return _dbContext.AnimeWithEmoji
                .Where(x => x.JapaneseTitle.ToLower().Contains(query) || x.Title.ToLower().Contains(query))
                .OrderBy(x => x.JapaneseTitle.Length)
                .Select(x => new AnimeFilter
                {
                    Id = x.Id,
                    Title = x.Title,
                    Thumbnail = x.Thumbnail,
                    MyanimeListId = x.MyanimeListId,
                    JapaneseTitle = x.JapaneseTitle,
                })
                .Take(4)
                .ToList();
        }



        public List<AnimeFilter> GetRandomAnimes()
        {
            var rnd = new Random();
            var anim = _dbContext.AnimeWithEmoji.Take(453).ToList()
                .OrderBy(item => rnd.Next())
                .Take(10)
                .Select(x => new AnimeFilter
                {
                    Id = x.Id,
                    Title = x.Title,
                    Thumbnail = x.Thumbnail,
                    Description = x.Description,
                    Image = x.Image,
                    properties = x.properties,
                    EmojiDescription = x.EmojiDescription,
                    MyanimeListId = x.MyanimeListId,
                })
                .ToList();

            anim.ForEach(a =>
            {
                var random = rnd.Next(0, anim.Count - 1);
                var gameType = rnd.Next(0, 4);
                a.Type = UtilityService.GetTypeByNumber(gameType);
            });

            return anim;
        }

        public dynamic GetEmojiQuiz(User user, string fingerPrint)
        {
            var guessGame = _cacheManager.GetCachedItem<GuessGame>("dailyGuess");

             if(guessGame == null)
            {
                return null;
            }

            var userGuessGame = _dbContext.UserGuessGames.FirstOrDefault(g => g.GuessGameId == guessGame.Id && g.UserId == user.Id);

            if (userGuessGame != null && (userGuessGame.Attempts >= 5 || userGuessGame.Result > 0))
            {
                return null;
            }

            int Attempts = userGuessGame?.Attempts ?? 0;

            return new { Id = guessGame.Id, AnimeId = guessGame.Anime.Id, guessGame.Anime.EmojiDescription, Attemps = Attempts };

        }

        public dynamic GetEmojiQuizUnAuthenticated(string fingerPrint)
        {
            var guessGame = _cacheManager.GetCachedItem<GuessGame>("dailyGuess");
       

            if (guessGame == null)
            {
                return null;
            }
            var userGuessGame = _dbContext.UserGuessGames.FirstOrDefault(u => u.GuessGame.Id == guessGame.Id && fingerPrint == u.Fingerprint);

            if (userGuessGame != null && (userGuessGame.Attempts >= 5 || userGuessGame.Result > 0))
            {
                return null;
            }

            int Attempts = userGuessGame?.Attempts ?? 0;


            return new { Id = guessGame.Id, AnimeId = guessGame.Anime.Id, guessGame.Anime.EmojiDescription, Attempts = Attempts };

        }
    }
}