using Microsoft.AspNetCore.Mvc;
using NHibernate;

using Animle;
using Animle.services;
using Animle.interfaces;
using Microsoft.AspNetCore.RateLimiting;

namespace StoryTeller.Controllers
{
    [EnableRateLimiting("fixed")]
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {

        private readonly ISessionFactory _sessionFactory;

        private readonly ILogger<UserController> _logger;
        
        private readonly TokenService _tokenService;


        public UserController(ILogger<UserController> logger, ISessionFactory sessionFactory, TokenService tokenService)
        {
            _tokenService = tokenService;
            _sessionFactory = sessionFactory;
            _logger = logger;
        }
       
        [HttpPost]
        public IActionResult CreateUser(User user)
        {
            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
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
                  
                    session.SaveOrUpdate(user);
                    transaction.Commit();
                    simpleResponse.Response = "User Created";
                    return Ok(simpleResponse);
                    
                } catch (Exception ex)
                {
                    transaction.Rollback();
                    if (ex.Message.Contains("Name") || ex.Message.Contains("Email"))
                    {
                        simpleResponse.Response = "Username or Password is taken";
                        return Conflict(simpleResponse);
                    }
                        


                    return StatusCode(500, "Internal server error");
                }
            
                
            }
        }

        [HttpGet(Name = "GetUsers")]
        public IEnumerable<User> Get()
        {
            using var session = _sessionFactory.OpenSession();
            using var transaction = session.BeginTransaction();
            IEnumerable<User> users = session.Query<User>().ToList();
            transaction.Commit();
            return users;
        }
        [Route("login")]
        [HttpPost]
        public IActionResult Login(LoginInfos loginInfos)
        {
            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var user = session.Query<User>().FirstOrDefault(u => u.Name == loginInfos.Name);
                if (user != null)
                {   
                    PasswordManager passwordManager = new PasswordManager();
                    

                    bool isAuthenticated = passwordManager.VerifyPassword(loginInfos.Password, user.Password);

                    if (isAuthenticated)
                    {
                        TokenResponse tokenResponse= new();
                        tokenResponse.Token = _tokenService.CreateToken(user);
                        return Ok(tokenResponse);
                    }
                    else
                    {
                        SimpleResponse response = new SimpleResponse();
                        response.Response = "\"Username or Password is incorrect";
                        return Unauthorized(response);
                    }
                }
                else
                {
                    return NotFound("User not found");
                }
            }
        }
        [Route("is-signed-in")]
        [HttpGet]
        public IActionResult IsSignedIn(int storyGroupId)
        {
            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                string token = HttpContext.Request.Headers["Authorization"];

                if(token == null)
                {
                    SimpleResponse response = new SimpleResponse();
                    response.Response = "Token expired on not valid!";
                    return BadRequest(response);

                }
                var claims = _tokenService.ValidateToken(token);
               
                if(claims == null)
                {
                    SimpleResponse response = new SimpleResponse();
                    response.Response = "Token expired";
                    return BadRequest(response);
                }
            
                return Ok(claims);
                
            }
        }
     
            }
     }
    
  
