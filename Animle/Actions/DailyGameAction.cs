using System.Security.Claims;
using Animle.Models;
using Animle.Services.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;

namespace Animle.Actions;

public class DailyGameAction : IAsyncActionFilter
{
    private readonly TokenService _tokenService;
    private readonly AnimleDbContext _context;

    public DailyGameAction(TokenService tokenService, AnimleDbContext context)
    {
        _tokenService = tokenService;
        _context = context;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var httpContext = context.HttpContext;
        var token = httpContext.Request.Headers["Authorization"].ToString();

        if (token.IsNullOrEmpty())
        {
            httpContext.Items["user"] = null;
     

        }
        else
        {

            var claims = _tokenService.ValidateToken(token);
            if(claims!= null)
            {
         

                var userNameClaim = claims.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);

                var user = _context.Users.FirstOrDefault(u => u.Name == userNameClaim.Value);
                if (user != null)
                {
                    httpContext.Items["user"] = user;
                   
                }


            }




        }








        await next();
    }
}