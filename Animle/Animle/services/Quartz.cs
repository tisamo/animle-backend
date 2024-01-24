using Animle.services;
using Quartz;
using System.Runtime.CompilerServices;

namespace Animle.NewFolder
{
    public static class Quartz
    {
        public static void RegisterCronJobs(this IServiceCollection services)
        {
            services.AddQuartz(options =>
            {
                var monthly = JobKey.Create("Monthly");
                var daily = JobKey.Create("Daily");
                var weekly = JobKey.Create("Weekly");
                var instant = JobKey.Create("Instant");
                options.UseMicrosoftDependencyInjectionJobFactory();
                options.AddJob<MonthlyJob>(monthly).AddTrigger(trigger =>
                {
                    trigger.ForJob(monthly).WithCronSchedule("* * 1 * * ?");
                });
               options.AddJob<MonthlyJob>(instant).AddTrigger(trigger =>
                {
                   trigger.ForJob(instant).StartNow();
               });
            });
            services.AddQuartzHostedService();
        }
    
    }
}
