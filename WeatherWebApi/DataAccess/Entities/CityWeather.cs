

using System.ComponentModel.DataAnnotations;

namespace WeatherApi.DataAccess.Entities
{
    public class CityWeather
    {
        [Key]
        public string CityKey { get; set; }
        public int CelsiusTemperature { get; set; }
  
        public string WeatherText { get; set; }

    }

}




