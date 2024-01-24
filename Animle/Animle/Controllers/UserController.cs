using Microsoft.AspNetCore.Mvc;
using NHibernate;

using Animle;
using Animle.services;
using Animle.interfaces;
using Microsoft.AspNetCore.RateLimiting;
using MySqlX.XDevAPI;

namespace StoryTeller.Controllers
{
    [EnableRateLimiting("fixed")]
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {

        private readonly ILogger<UserController> _logger;

        private readonly TokenService _tokenService;
        private readonly AnimleDbContext _animleDbContext;


        public UserController(ILogger<UserController> logger, TokenService tokenService, AnimleDbContext animleDbContext)
        {
            _tokenService = tokenService;
            _logger = logger;
            _animleDbContext = animleDbContext;
        }

        [HttpPost]
        public IActionResult CreateUser(User user)
        {

            SimpleResponse simpleResponse = new SimpleResponse();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                PasswordManager passwordManager = new PasswordManager();
                user.Password = passwordManager.HashPassword(user.Password);
                user.Rating = 1000;
                _animleDbContext.Users.Add(user);
                _animleDbContext.SaveChanges();
                simpleResponse.Response = "User Created";
                return Ok(simpleResponse);

            }
            catch (Exception ex)
            {

                if (ex.Message.Contains("Name") || ex.Message.Contains("Email"))
                {
                    simpleResponse.Response = "Username or Password is taken";
                    return Conflict(simpleResponse);
                }

                return StatusCode(500, "Internal server error");
            }



        }


        [Route("login")]
        [HttpPost]
        public IActionResult Login(LoginInfos loginInfos)
        {


            var user = _animleDbContext.Users.FirstOrDefault(u => u.Name == loginInfos.Name);
            if (user != null)
            {
                PasswordManager passwordManager = new PasswordManager();


                bool isAuthenticated = passwordManager.VerifyPassword(loginInfos.Password, user.Password);

                if (isAuthenticated)
                {
                    TokenResponse tokenResponse = new();
                    tokenResponse.Token = _tokenService.CreateToken(user);

                    return Ok(tokenResponse);
                }
                else
                {
                    SimpleResponse response = new SimpleResponse();
                    response.Response = "Username or Password is incorrect!";
                    return Unauthorized(response);
                }
            }
            else
            {
                SimpleResponse response = new SimpleResponse();
                response.Response = "User not found!";
                return NotFound(response);
            }

        }
        [Route("is-signed-in")]
        [HttpGet]
        public IActionResult IsSignedIn()
        {

            string token = HttpContext.Request.Headers["Authorization"];

            SimpleResponse response = new SimpleResponse();

            if (token == null)
            {
                response.Response = "You have to login to access this page!";

                return BadRequest(response);

            }
            var claims = _tokenService.ValidateToken(token);

            if (claims == null)
            {
                response.Response = "Token expired";

                return Unauthorized(response);
            }

            var user = _tokenService.GetUser(_animleDbContext, claims);

            response.Response = user.Name;

            return Ok(response);


        }

    }
}


