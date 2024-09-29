using System.Security.Claims;
using Animle.Models;
using Animle.Services.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Animle.Actions;

public class CustomAuthorizationFilter : IAsyncActionFilter
{
    private readonly TokenService _tokenService;
    private readonly AnimleDbContext _context;

    public CustomAuthorizationFilter(TokenService tokenService, AnimleDbContext context)
    {
        _tokenService = tokenService;
        _context = context;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var httpContext = context.HttpContext;
        var token = httpContext.Request.Headers["Authorization"].ToString();

        if (string.IsNullOrEmpty(token))
        {
            context.Result = new UnauthorizedObjectResult(new { Response = "Login required" });
            return;
        }

        var claims = _tokenService.ValidateToken(token);

        if (claims == null)
        {
            context.Result = new UnauthorizedObjectResult(new { Response = "Token invalid" });
            return;
        }

        var userNameClaim = claims.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);

        var user = _context.Users.FirstOrDefault(u => u.Name == userNameClaim.Value);
        if (user == null)
        {
            context.Result = new UnauthorizedObjectResult(new { Response = "Token invalid" });
            return;
        }

        httpContext.Items["user"] = user;

        await next();
    }
}