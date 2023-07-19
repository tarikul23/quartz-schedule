using Microsoft.AspNetCore.Mvc;
using Quartz;
using QuartzClient;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<ITestService, TestService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionScopedJobFactory();
    //q.UsePersistentStore(s =>
    //{
    //    s.UseSqlServer("Server=localhost,1433;Database=Quartz;User Id=sa;Password=<CONNECTION_STRING>;Encrypt=False;");
    //});

    q.UseInMemoryStore();

    //q.AddJobAndTrigger<HelloWorldJob>("0/1 * * * * ?");
});
builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => "Hello World!");
app.MapGet("/schedule", async ([FromServices]ISchedulerFactory schedulerFactory) => {

    IJobDetail job = JobBuilder.Create<HelloWorldJob>().WithIdentity("job1", "group1").Build();

    ITrigger trigger = TriggerBuilder.Create()
                     .WithIdentity("trigger1", "group1")
                     .StartNow()
                     .WithSimpleSchedule(x => x
                      .WithIntervalInSeconds(2)
                      .RepeatForever())
                     .Build();
    IScheduler scheduler = await schedulerFactory.GetScheduler();
    await scheduler.ScheduleJob(job, trigger);
});
app.MapGet("/immediately", async ([FromServices] ISchedulerFactory schedulerFactory) => {

    IJobDetail job = JobBuilder.Create<HelloWorldJob>().WithIdentity("job1", "group1").Build();

    ITrigger trigger = TriggerBuilder.Create().WithIdentity("trigger1", "group1").StartNow().Build();

    IScheduler scheduler = await schedulerFactory.GetScheduler();
    await scheduler.ScheduleJob(job, trigger);
});

app.MapGet("/delay", async ([FromServices] ISchedulerFactory schedulerFactory) => {

    IJobDetail job = JobBuilder.Create<HelloWorldJob>().WithIdentity("job1", "group1").Build();

    ITrigger trigger = TriggerBuilder.Create()
    .WithIdentity("trigger5", "group1")
    .StartAt(DateBuilder.FutureDate(5, IntervalUnit.Second)) 
    .ForJob(job)
    .Build();

    IScheduler scheduler = await schedulerFactory.GetScheduler();
    await scheduler.ScheduleJob(job, trigger);
});
app.MapGet("/call-test-service", async ([FromServices] ISchedulerFactory schedulerFactory) => {

    IJobDetail job = JobBuilder.Create<TestJob>().WithIdentity("job1", "group1").Build();

    ITrigger trigger = TriggerBuilder.Create().WithIdentity("trigger1", "group1").StartNow().Build();

    IScheduler scheduler = await schedulerFactory.GetScheduler();
    await scheduler.ScheduleJob(job, trigger);
});
app.MapGet("/call-test-api", async ([FromServices] ISchedulerFactory schedulerFactory) => {

    IJobDetail job = JobBuilder.Create<CallApiJob>().WithIdentity("job1", "group1").Build();

    ITrigger trigger = TriggerBuilder.Create()
                     .WithIdentity("trigger1", "group1")
                     .StartNow()
                     .WithSimpleSchedule(x => x
                      .WithIntervalInSeconds(2)
                      .RepeatForever())
                     .Build();

    IScheduler scheduler = await schedulerFactory.GetScheduler();
    await scheduler.ScheduleJob(job, trigger);
});

app.Run();