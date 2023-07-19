using Quartz;

namespace QuartzClient
{
    [DisallowConcurrentExecution]
    public class TestJob : IJob
    {
        private readonly ITestService _testService;
        public TestJob(ITestService testService)
        {
            _testService = testService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _testService.RunAsync();
        }
    }
}
