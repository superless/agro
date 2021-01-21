using System.Threading.Tasks;
using trifenix.connect.agro_model;

namespace trifenix.connect.agro.interfaces
{
    public interface IWeatherApi {
        Task<Weather> GetWeather(float lat, float lon);
    }

}