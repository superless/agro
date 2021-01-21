namespace trifenix.connect.agro_model
{
    public class Weather {

        public Coordinates Coordinates { get; set; }
        public Wind Wind { get; set; }
        public string Main { get; set; }
        public string Description { get; set; }
        private float _temperatureCelcius;
        public float TemperatureCelcius { get => _temperatureCelcius; set => _temperatureCelcius = value - (float)273.15; }
        public int CloudsPercentage { get; set; }
        public int HumidityPercentage { get; set; }
        public int PressureHectoPascal { get; set; }
        private string _urlIcon;
        public string UrlIcon { get => _urlIcon; set => _urlIcon = "https://openweathermap.org/themes/openweathermap/assets/vendor/owm/img/widgets/" + value + ".png"; }

        
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