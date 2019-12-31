namespace trifenix.agro.weather.model {
    public class Weather {
        public Coordinates Coordinates;
        public Wind Wind;
        public string Main;
        public string Description;
        private float _temperatureCelcius;
        public float TemperatureCelcius { get => _temperatureCelcius; set => _temperatureCelcius = value - (float)273.15; }
        public int CloudsPercentage;
        public int HumidityPercentage;
        public int PressureHectoPascal;
        private string _urlIcon;
        public string UrlIcon { get => _urlIcon; set => _urlIcon = "https://openweathermap.org/themes/openweathermap/assets/vendor/owm/img/widgets/" + value + ".png"; }

        public Weather(string cityName, float lat, float lon, string main, string desc, float temp, float speed, int degree, int cloud, int hum, int pressure, string iconCode) {
            Coordinates = new Coordinates() { CityName = cityName, Latitude = lat, Longitude = lon };
            Wind = new Wind() { Speed = speed, Degree = degree };
            Main = main;
            Description = desc;
            TemperatureCelcius = temp;
            CloudsPercentage = cloud;
            HumidityPercentage = hum;
            PressureHectoPascal = pressure;
            UrlIcon = iconCode;
        }
    }

    public class Coordinates {
        public string CityName;
        public float Latitude;
        public float Longitude;
    }

    public class Wind {
        public float Speed;
        public int Degree;
    }

}