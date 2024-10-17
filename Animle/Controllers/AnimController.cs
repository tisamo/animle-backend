using Animle.Actions;
using Animle.Classes;
using Animle.Models;
using Animle.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using MySqlX.XDevAPI.Common;

namespace Animle.Controllers
{
    [Route("anime")]
    [ApiController]
    public class AnimController : ControllerBase
    {
        private readonly IAnimeService _animeService;
        private readonly EncryptionHelper _encryptionHelper;

        public AnimController(IAnimeService animeService, EncryptionHelper encryptionHelper)
        {
            _animeService = animeService;
            _encryptionHelper = encryptionHelper;
        }

        [HttpGet]
        [ServiceFilter(typeof(DailyGameAction))]
        [EnableRateLimiting("fixed")]
        [Route("daily/{fingerprint}")]
        public async Task<IActionResult> Daily(string fingerprint)
        {
               var user = HttpContext.Items["user"] as User;
           
             var  result = await _animeService.GetDailyChallengeAsync(user, fingerprint);
            
         
            return result != null ? Ok(result) : BadRequest(new { Response = "You have already played this game."});

        }

        [HttpPost]
        [EnableRateLimiting("fixed")]
        [ServiceFilter(typeof(DailyGameAction))]
        [Route("contest/{type}")]
        public async Task<IActionResult> DailyResult(string type, DailyGameResult gameResult)
        {
            User? currentUser = HttpContext.Items["user"] as User;

            dynamic result;

            if(currentUser != null)
            {
                 result = await _animeService.SubmitGameResultAsync(type, currentUser, gameResult);

            }
            else
            {
                result = await _animeService.SubmitGameResultAnonymously(type, gameResult);

            }

            return result.IsSuccess ? Ok(result) : BadRequest(result.Response);
            

        }

        [HttpPost]
        [EnableRateLimiting("fixed")]
        [ServiceFilter(typeof(DailyGameAction))]
        [Route("guess/progress")]
        public async Task<IActionResult> GuessGameProgress(GuessGameProgress gameResult)
        {
            User? currentUser = HttpContext.Items["user"] as User;

            dynamic result;

            if (currentUser!= null)
            {
                result = await _animeService.SubmitLoggedInUserProgress(currentUser, gameResult);
            }
            else
            {
                result = await _animeService.SubmitUnAuthenticatedUserProgress(gameResult);
            }


                return result.IsSuccess ? Ok(result) : BadRequest(result.Response);
            
        }



        [HttpGet]
        [EnableRateLimiting("fixed")]
        [Route("filter")]
        public async Task<IActionResult> SearchAnime([FromQuery] string q)
        {
            var filteredList = await _animeService.FilterAnimeAsync(q);
            return Ok(filteredList);
        }

        [HttpGet]
        [EnableRateLimiting("fixed")]
        [Route("Random")]
        public IActionResult Random()
        {
            var randomAnimes = _animeService.GetRandomAnimes();
            return Ok(randomAnimes);
        }

        [HttpGet]
        [EnableRateLimiting("fixed")]
        [ServiceFilter(typeof(DailyGameAction))]
        [Route("emoji-quiz/{fingerPrint}")]
        public IActionResult EmojiQuiz(string fingerPrint)
        {
            dynamic quiz;

            string encodedFingerprint = Uri.EscapeDataString(fingerPrint);
            if ( HttpContext.Items["user"] is User user)
            {
                 quiz = _animeService.GetEmojiQuiz(user, fingerPrint);
            }
            else
            {
                quiz = _animeService.GetEmojiQuizUnAuthenticated(fingerPrint);
            }


                return quiz != null ? Ok(new { Response = _encryptionHelper.Encrypt(quiz) }) : BadRequest(new { Response = "You have already played this game." });
            

        }
    }
}