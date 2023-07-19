using Quartz;

namespace QuartzClient
{
    [DisallowConcurrentExecution]
    public class CallApiJob : IJob
    {
        private readonly ITestService _testService;
        public CallApiJob(ITestService testService)
        {
            _testService = testService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _testService.CallApiAsync();
        }
    }
}
