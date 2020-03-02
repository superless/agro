using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using trifenix.agro.translator.operations;
using trifenix.agro.weather.interfaces;
using trifenix.agro.weather.model;

namespace trifenix.agro.weather.operations {
    public class WeatherApi : IWeatherApi {

        private readonly string _appId;

        public WeatherApi(string appId) {
            _appId = appId;
        }

        public async Task<Weather> GetWeather(float lat, float lon) {
            HttpClient client = new HttpClient();
            GoogleTranslator translator = new GoogleTranslator();
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, "https://api.openweathermap.org/data/2.5/weather?lat=" + lat + "&lon=" + lon + "&appid=" + _appId);
            var response = await client.SendAsync(requestMessage);
            client.Dispose();
            var responseBody = await response.Content.ReadAsStringAsync();
            dynamic json = JsonConvert.DeserializeObject(responseBody);
            string cityName = (string)json.name;
            string main = (string)json.weather[0].main;
            string desc = (string)json.weather[0].description;
            float temp = (float)json.main.temp;
            float speed = (float)json.wind.speed;
            int degree = (int)json.wind.degree;
            int cloud = (int)json.clouds;
            int hum = (int)json.main.humidity;
            int pressure = (int)json.main.pressure;
            string iconCode = (string)json.weather[0].icon;
            return new Weather {
                Coordinates = new Coordinates() { CityName = cityName, Latitude = lat, Longitude = lon },
                Wind = new Wind() { Speed = speed, Degree = degree },
                Main = main,
                Description = desc,
                TemperatureCelcius = temp,
                CloudsPercentage = cloud,
                HumidityPercentage = hum,
                PressureHectoPascal = pressure,
                UrlIcon = iconCode
            };
        }

    }
}