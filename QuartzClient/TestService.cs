namespace QuartzClient
{
    public interface ITestService
    {
        Task RunAsync();
        Task CallApiAsync();
    }
    public class TestService : ITestService
    {
        public async Task CallApiAsync()
        {
            Console.WriteLine("----------------Start---------------");
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7273/");
            var data = await client.GetFromJsonAsync<IEnumerable<WeatherForecast>>("WeatherForecast");
            foreach (var item in data ?? new List<WeatherForecast>())
            {
                Console.WriteLine($"Date: {item.Date} - TemperatureC: {item.TemperatureF}");
            }
            Console.WriteLine("----------------End---------------");
        }

        public Task RunAsync()
        {
            Console.WriteLine($"test service run {DateTime.Now}");
            return Task.CompletedTask;
        }
    }

    public class WeatherForecast
    {
        public DateOnly Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string? Summary { get; set; }
    }

}
