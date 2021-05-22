

using System.ComponentModel.DataAnnotations;

namespace WeatherApi.DataAccess.Entities
{
    public class Favorite
    {
        [Key]
        public string CityKey { get; set; }
        public string LocalizedName { get; set; }
    }

}




