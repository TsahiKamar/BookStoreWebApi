using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeatherApi.Models;

namespace WeatherApi.Services
{
    public class WeatherService
    {
        const string ApiKey = "m0XQhZB6q0A6ztq0GGWiBJpRRvdDQVXF";

        ///locations/v1/cities/autocomplete?apikey=BJLiRte3ZRqdXa6GshrLml2hN5VoeQ2O&q=osa HTTP/1.1
        public async Task<List<AutocompleteModel>> LocationAutocomplete(string q)
        {
            var url = "https://dataservice.accuweather.com/locations/v1/cities/autocomplete?apikey=" + ApiKey + "&" + "q=" + q;

            List<AutocompleteModel> autocomplete = new List<AutocompleteModel>();
            try 
            { 

                using (var httpClient = new System.Net.Http.HttpClient())
                {
                    using (var response = await httpClient.GetAsync(url)) 
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        autocomplete = JsonConvert.DeserializeObject<List<AutocompleteModel>>(apiResponse);
                    }
                }
            }
            catch(Exception ex) 
            {
                throw ex;
            }
            return autocomplete;
        }

        //http://localhost:58365/weather/GetCurrentWeather?q=225007
        //http://dataservice.accuweather.com/currentconditions/v1/123?apikey=BJLiRte3ZRqdXa6GshrLml2hN5VoeQ2O
        public async Task<dynamic> GetCurrentConditionsAsync(string locationKey)
        {
            var url = "https://dataservice.accuweather.com/currentconditions/v1/" + locationKey + "?" + "apikey=" + ApiKey;
            IList<CurrentConditionsModel> currentConditions = new List<CurrentConditionsModel>();

            dynamic res;
            try
            {
                using (var httpClient = new System.Net.Http.HttpClient())
                {
                    using (var response = await httpClient.GetAsync(url))
                    {
                        res = await response.Content.ReadAsStringAsync();
                    }
                }
            }
            catch(Exception ex) 
            {
                throw ex;
            }

            return res; 
        }

    }
}



