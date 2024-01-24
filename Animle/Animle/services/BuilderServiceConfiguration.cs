using Animle.NewFolder;
using AspNetCoreRateLimit;
using NHibernate;
using System.Text.Json.Serialization;

namespace Animle.services
{
    public static class BuilderServiceConfiguration
    {
        public static void ConfigureServices(WebApplicationBuilder builder)
        {
            var sessionFactory = NHibernateHelper.CreateSessionFactory();
            var configuration = new ConfigurationBuilder()
             .AddJsonFile("./appsettings.json")
              .Build();
            builder.Services.AddMemoryCache();
            builder.Services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));
            builder.Services.Configure<IpRateLimitPolicies>(configuration.GetSection("IpRateLimitPolicies"));
            builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            builder.Services.AddControllers();
            builder.Services.AddSignalR();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.RegisterCronJobs();
            builder.Services.AddSwaggerGen();
            builder.Services.AddHttpClient();
            builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
            builder.Services.AddSingleton<RequestCacheManager>();
            builder.Services.AddSingleton<ISessionFactory>(sessionFactory);
            builder.Services.AddSingleton<TokenService>(provider =>
            {
                var secretKey = configuration.GetSection("AppSettings:SecretKey").Value;

                return new TokenService(secretKey);
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:4200") // Allow requests from this origin
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
            });

            // Other configurations...
        }
    }
}
