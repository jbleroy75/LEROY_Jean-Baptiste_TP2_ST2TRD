using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OpenWeather
{
    internal class Program
    {
        static string apiKey = "062c4d37b7308b94567283d0f576e88b";

        static void Main(string[] args)
        {
            try
            {
                Console.Write("What’s the weather like in Morocco? ");
                var root = processWeatherAsync(31.7917, 7.0926).Result; // Morocco
                Console.WriteLine(root.current.weather[0].description);

                Console.WriteLine();
                Console.WriteLine("When will the sun rise and set today in Oslo? ");
                root = processWeatherAsync(59.9139, 10.7522).Result; // OSlo
                var dateRise = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).AddSeconds(root.current.sunrise);
                var dateSet = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).AddSeconds(root.current.sunset);
                Console.WriteLine($"sunrise: {dateRise.ToShortTimeString()} sunset: {dateSet.ToShortTimeString()}");

                Console.WriteLine();
                Console.Write("What’s the temperature in Jakarta (in Celsius)? ");
                root = processWeatherAsync(6.2088, 106.8456).Result; // Jakarta
                Console.WriteLine($"{root.current.temp} Celsius");

                Console.WriteLine();
                Console.Write("Where is it more windy, New-York, Tokyo or Paris? ");
                var newYork = processWeatherAsync(40.7128, 74.0060).Result.current.wind_speed;
                var tokyo = processWeatherAsync(35.6762, 139.6503).Result.current.wind_speed;
                var paris = processWeatherAsync(48.8566, 2.3522).Result.current.wind_speed;
                var moreWindy = string.Empty;

                if (newYork > paris && newYork > tokyo)
                    moreWindy = "New-York";
                else if (paris > tokyo && paris > newYork)
                    moreWindy = "Paris";
                else if (tokyo > paris && tokyo > newYork)
                    moreWindy = "Tokyo";
                Console.WriteLine(moreWindy);

                Console.WriteLine();
                Console.WriteLine("What is the humidity and pressure like in Kiev*, Moscow* and Berlin?");

                var Kiev = processWeatherAsync(50.4501, 30.5234).Result;
                Console.WriteLine($"Kiev  Humidity: {Kiev.current.humidity}% Pressure: {Kiev.current.pressure} hpa");

                var Moscow = processWeatherAsync(55.7558, 37.6173).Result;
                Console.WriteLine($"Moscow  Humidity: {Moscow.current.humidity}% Pressure: {Moscow.current.pressure} hpa");

                var Berlin = processWeatherAsync(52.5200, 13.4050).Result;
                Console.WriteLine($"Berlin  Humidity: {Berlin.current.humidity}% Pressure: {Berlin.current.pressure} hpa");

                Console.WriteLine("Completed");
                Console.ReadLine();

            }
            catch (Exception)
            {
                Console.WriteLine("running on free openweathermap plan. wait for a min");
            }
        }

        async static Task<Root> processWeatherAsync(double lat, double lon)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://api.openweathermap.org/data/2.5/onecall?lat={lat}&lon={lon}&appid={apiKey}&units=metric&exclude=hourly,daily,minutely")
            };

            try
            {
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var jsonStr = await response.Content.ReadAsStringAsync();
                    Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(jsonStr);
                    return myDeserializedClass;
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

    }
}