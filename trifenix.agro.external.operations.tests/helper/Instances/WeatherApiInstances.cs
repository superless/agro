using Moq;
using System;
using trifenix.agro.common.tests.fakes;
using trifenix.agro.external.operations.tests.helper.Moqs;
using trifenix.agro.weather.interfaces;
using trifenix.agro.weather.model;

namespace trifenix.agro.external.operations.tests.helper.Instances
{

    public static class WeatherApiInstances
    {
        public static Mock<IWeatherApi> GetInstance(){
            var mockWeatherApi = new Mock<IWeatherApi>();
            mockWeatherApi.Setup(s => s.GetWeather(It.IsAny<float>(), It.IsAny<float>())).ReturnsAsync(It.IsAny<Weather>());
            return mockWeatherApi;
        }
    }
}