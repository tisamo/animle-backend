﻿using System.Text.Json.Serialization;
using System.Threading.RateLimiting;
using Animle.Actions;
using Animle.Classes;
using Animle.Helpers;
using Animle.Interfaces;
using Animle.Services;
using Animle.Services.Auth;
using Animle.Services.Cache;
using Animle.Services.Email;
using Animle.Services.Quartz;
using Animle.SignalR;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Animle
{
    public static class BuilderServiceConfiguration
    {
        public static void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddRateLimiter(opt =>
            {
                opt.AddFixedWindowLimiter(policyName: "fixed", options =>
                {
                    options.PermitLimit = 6;
                    options.Window = TimeSpan.FromSeconds(5);
                    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    options.QueueLimit = 10;
                });
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.RegisterCronJobs();
            builder.Services.AddSwaggerGen();
            builder.Services.AddHttpClient();
            builder.Services.Configure<ConfigSettings>(builder.Configuration.GetSection("AppSettings"));

            builder.Services.AddDbContext<AnimleDbContext>((serviceProvider, options) =>
            {
                var configSettings = serviceProvider.GetRequiredService<IOptions<ConfigSettings>>().Value;
                var dbConnect = configSettings.DbConnection;
                options.UseMySql(dbConnect, ServerVersion.AutoDetect(dbConnect));
            });


            builder.Services.AddSignalR();
            builder.Services.AddSingleton<EncryptionHelper>();
            builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
            builder.Services.AddSingleton<SignalrAnimeService>();
            builder.Services.AddSingleton<IRequestCacheManager, RequestCacheManager>();
            builder.Services.AddSingleton<EmailService>();
            builder.Services.AddScoped<NotificationService>();

            builder.Services.AddScoped<CustomAuthorizationFilter>();
            builder.Services.AddScoped<DailyGameAction>();


            builder.Services.AddScoped<IAnimeService, AnimeService>();
            builder.Services.AddScoped<IQuizService, QuizService>();
            builder.Services.AddScoped<IUserService, UserService>();


            builder.Services.AddSingleton<TokenService>(provider =>
            {
                var configSettings = provider.GetRequiredService<IOptions<ConfigSettings>>().Value;

                var secretKey = configSettings.SecretKey;

                return new TokenService(secretKey);
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:4200")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });
        }
    }
}