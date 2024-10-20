﻿using Animle.Actions;
using Animle.Classes;
using Animle.Helpers;
using Animle.Models;
using Animle.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using NHibernate.Util;

namespace Animle.Controllers;

[Route("quiz")]
[ApiController]
public class QuziController : ControllerBase
{
    private readonly IQuizService _quizService;

    public QuziController(IQuizService quizService)
    {
        _quizService = quizService;
    }

    [EnableRateLimiting("fixed")]
    [ServiceFilter(typeof(CustomAuthorizationFilter))]
    [HttpPost]
    public async Task<IActionResult> PostQuiz([FromBody] QuizCreation quiz)
    {
        if (HttpContext.Items["user"] is User user)
        {
            try
            {
                await _quizService.CreateQuizAsync(quiz, user);
                return Ok(new SimpleResponse { Response = "Quiz Created" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        else
        {
            return Unauthorized(new { Response = "Please login first!" });
        }
    }

    [EnableRateLimiting("fixed")]
    [ServiceFilter(typeof(CustomAuthorizationFilter))]
    [HttpPut]
    public async Task<IActionResult> EditQuiz([FromBody] QuizCreation quiz)
    {
        if (HttpContext.Items["user"] is User user)
        {
            try
            {
                var response = await _quizService.EditQuiZAsync(quiz, user);
                return response.IsSuccess ? Ok(new SimpleResponse { Response = response.Response }): BadRequest(response.Response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        else
        {
            return Unauthorized(new { Response = "Please login first!" });
        }
    }


    [EnableRateLimiting("fixed")]
    [ServiceFilter(typeof(CustomAuthorizationFilter))]
    [Route("like/{id}")]
    [HttpGet]
    public async Task<IActionResult> LikeQuiz(int id)
    {
        if (HttpContext.Items["user"] is User user)
        {
            try
            {
                var response = await _quizService.LikeQuizAsync(id, user);
                return Ok(new SimpleResponse { Response = response });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        return Unauthorized(new { Response = "You must be logged in" });
    }

    [HttpGet]
    [ServiceFilter(typeof(DailyGameAction))]
    [Route("likes")]
    public async Task<IActionResult> RetrieveUserLikes()
    {
        if (HttpContext.Items["user"] is User user)
        {
            try
            {
                var likes = await _quizService.RetrieveUserLikesAsync(user);
                return Ok(likes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        else
        {
            dynamic[] array = [];
            return Ok(array);
        }
    }

    [EnableRateLimiting("fixed")]
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var quiz = await _quizService.GetQuizByIdAsync(id);
        var rnd = new Random();
        for(var i = 0; i < quiz.Animes.Count; i++)
        {
            quiz.Animes.ElementAt(i).Type = UtilityService.GetTypeByNumber(rnd.Next(0, 4));

        }

        return Ok(quiz);
    }

    [EnableRateLimiting("fixed")]
    [ServiceFilter(typeof(CustomAuthorizationFilter))]
    [HttpGet("edit/{id}")]
    public async Task<IActionResult> GetUserQuiz(int id)
    {
       if (HttpContext.Items["user"] is User user)
        {
            try
            {
                var response = await _quizService.GetUserQuizForEditing(id, user);
                if (response == null)
                {
                    return BadRequest(new {Response  = "You can't edit this quiz"});
                }    
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        return Unauthorized(new { Response = "You must be logged in" });
    }
    

    [EnableRateLimiting("fixed")]
    [HttpGet]
    public async Task<IActionResult> GetQuizzes()
    {
        var queryString = Request.Query;
        var quizzes = await _quizService.GetQuizzesAsync(queryString);
        return Ok(quizzes);
    }
}