using System;
using System.Reflection;
using System.Web;
using Animle.Classes;
using Animle.Helpers;
using Animle.Models;
using FluentNHibernate.Conventions;
using Microsoft.EntityFrameworkCore;

namespace Animle.Services;

public class QuizDto
{
    public int Id { get; set; }

    public List< QuizLikes>? Likes { get; set; }
    public string Title { get; set; }
    public DateTime CreatedAt { get; set; }

    public string Thumbnail { get; set; }

    public UserDto user { get; set; }
}

public interface IQuizService
{
    Task<Quiz> CreateQuizAsync(QuizCreation quiz, User user);
    Task<SimpleResponse> EditQuiZAsync(QuizCreation quiz, User user);
    Task<string> LikeQuizAsync(int id, User user);
    Task<List<int>> RetrieveUserLikesAsync(User user);
    Task<Quiz> GetQuizByIdAsync(int id);
    Task<dynamic> GetUserQuizForEditing(int id, User user);
    Task<ListResponse<QuizDto>> GetQuizzesAsync(IQueryCollection queryString);
}

public class QuizService : IQuizService
{
    private readonly AnimleDbContext _context;

    public QuizService(AnimleDbContext context)
    {
        _context = context;
    }

    public async Task<Quiz> CreateQuizAsync(QuizCreation quiz, User user)
    {
        var newQuiz = new Quiz
        {
            Title = quiz.Title,
            CreatedAt = DateTime.Now,
            user = user,
            Animes = _context.AnimeWithEmoji
                .Where(p => quiz.AnimeIds.Contains(p.MyanimeListId))
                .ToList(),
            Thumbnail = _context.AnimeWithEmoji
                .FirstOrDefault(a => a.MyanimeListId == quiz.SelectedImageId)?.Thumbnail
        };

        _context.Quizes.Add(newQuiz);
        await _context.SaveChangesAsync();
        return newQuiz;
    }


    public async Task<SimpleResponse> EditQuiZAsync(QuizCreation quiz, User user)
    {
        var quizToEdit = _context.Quizes
         .Include(q => q.Animes)
         .Include(q => q.user) 
         .FirstOrDefault(q => q.Id == quiz.Id && q.user.Id == user.Id);

        if (quizToEdit == null)
        {
            return new SimpleResponse { IsSuccess = false, Response = "You can't edit this quiz" };
        }

        quizToEdit.Title = quiz.Title;

        quizToEdit.Animes = _context.AnimeWithEmoji
            .Where(p => quiz.AnimeIds.Contains(p.MyanimeListId))
            .ToList();

        quizToEdit.Thumbnail = _context.AnimeWithEmoji
            .FirstOrDefault(a => a.MyanimeListId == quiz.SelectedImageId)?.Thumbnail;

        _context.Quizes.Update(quizToEdit);
        await _context.SaveChangesAsync();

        return new SimpleResponse { IsSuccess = true, Response = "Quiz successfully edited" };
    }

    public async Task<string> LikeQuizAsync(int id, User user)
    {
        var quiz = await _context.Quizes.FindAsync(id);
        if (quiz == null) return "Quiz not found";

        var existingLike = await _context.QuizLikes
            .FirstOrDefaultAsync(q => q.quizId == id && q.UserId == user.Id);

        if (existingLike != null)
        {
            _context.QuizLikes.Remove(existingLike);
            await _context.SaveChangesAsync();
            return "Quiz Removed";
        }

        var quizLike = new QuizLikes { LikedQuiz = quiz, User = user };
        _context.QuizLikes.Add(quizLike);
        await _context.SaveChangesAsync();
        return "Quiz Added To user";
    }

    public async Task<List<int>> RetrieveUserLikesAsync(User user)
    {
        return await _context.QuizLikes
            .Where(q => q.UserId == user.Id)
            .Select(x => x.quizId)
            .ToListAsync();
    }

    public async Task<Quiz> GetQuizByIdAsync(int id)
    {
        return await _context.Quizes
            .Include(x => x.Animes)
            .FirstOrDefaultAsync(x => x.Id == id);
    }



    public async Task<dynamic> GetUserQuizForEditing(int id, User user)
    {
        var quiz = await _context.Quizes
            .Include(x => x.Animes)
                .Select(x=>
               new{
                 Animes = x.Animes,
                 Id = x.Id,
                 UserId = x.UserId,
                 Title = x.Title,
                 CreatedAt = x.CreatedAt,
                 Thumbnail = x.Thumbnail
                })
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == user.Id);
        return quiz;   
    }


    public async Task<ListResponse<QuizDto>> GetQuizzesAsync(IQueryCollection queryString)
    {
            queryString.TryGetValue("user", out var userValue);
            queryString.TryGetValue("top", out var onlyTop);



        var query = UtilityService.GenerateQuery(queryString,
            _context.Quizes.Include(x => x.user).Include(x=> x.Likes).Select(x => new QuizDto
            {
                Thumbnail = x.Thumbnail,
                Title = x.Title,
                CreatedAt = x.CreatedAt,
                Id = x.Id,
                Likes = (List<QuizLikes>)x.Likes,
                user = new UserDto
                {
                    Name = x.user.Name,
                    Id = x.user.Id
                }
            }).ToList());

        if (!onlyTop.IsEmpty()) {
            query.List = query.List.OrderByDescending(x => x.Likes.Count).ToList();
        }

        if (!userValue.IsEmpty())
        {
   
            query.List = query.List.Where(x => x.user.Id.ToString() ==userValue).ToList();
        }




        return query;
    }
}