using Quartz;

namespace QuartzClient
{
    public static class ServiceCollectionQuartzConfiguratorExtensions
    {
        public static void AddJobAndTrigger<T>(this IServiceCollectionQuartzConfigurator quartz, string cronSchedule) where T : IJob
        {
            string jobName = typeof(T).Name;

            var jobKey = new JobKey(jobName);
            quartz.AddJob<T>(opts => opts.WithIdentity(jobKey));

            quartz.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity(jobName + "-trigger")
                .WithCronSchedule(cronSchedule));
        }
    }
}
