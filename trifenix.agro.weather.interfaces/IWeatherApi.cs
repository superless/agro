using System.Threading.Tasks;
using trifenix.agro.weather.model;

namespace trifenix.agro.weather.interfaces {
    public interface IWeatherApi {
        Task<Weather> GetWeather(float lat, float lon);
    }

}