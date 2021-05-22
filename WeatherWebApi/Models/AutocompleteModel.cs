
namespace WeatherApi.Models
{
    public class AutocompleteModel
    {
        public int Version { get; set; }
        public string Key { get; set; }
        public string Type { get; set; }
        public int Rank { get; set; }
        public string LocalizedName { get; set; }
        public _Country Country { get; set; }
        public _AdministrativeArea AdministrativeArea { get; set; }
        
    }

    public class _Country {
        public string ID { get; set; }
        public string LocalizedName { get; set; }
    
    }

    public class _AdministrativeArea {
        public string ID { get; set; }
        public string LocalizedName { get; set; }
    }

}


