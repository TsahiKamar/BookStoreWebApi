using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeatherApi.Dal;
using WeatherApi.DataAccess.Entities;
using WeatherApi.Models;
using WeatherApi.Services;

namespace WeatherWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherController : ControllerBase 
    {
 
        private readonly ILogger<WeatherController> _logger;

        private readonly DatabaseContext db;
        public WeatherController( ILogger<WeatherController> logger, DatabaseContext context)
        {
            _logger = logger;
            db = context;
        }

        //http://localhost:58365/weather/search?q=osa
        [HttpGet("Search")]
        public async Task<ActionResult<List<WeatherApi.Models.AutocompleteModel>>> LocationAutocomplete(string q)
        {
            List<WeatherApi.Models.AutocompleteModel> response = null;
            try
            {
                 var weatherService = new WeatherService();
                response = await weatherService.LocationAutocomplete(q); 
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("LocationAutocomplete error : ", ex.ToString(), EventLogEntryType.Error);
                return StatusCode(500); //Internal Server Error

            }
            return Ok(response);
        }

        //http://localhost:58365/weather/GetCurrentWeather?q=225007
        [HttpGet("GetCurrentWeather")]
        public async Task<ActionResult<dynamic>> GetCurrentConditions(string q)
        {
            dynamic res;
            try
            {
                var cityWeather = db.CitiesWeather.Where(s => s.CityKey == q)
                    .FirstOrDefault();

                if (cityWeather == null)
                {
                    var weatherService = new WeatherService();
                    res = await weatherService.GetCurrentConditionsAsync(q);
 
                }
                else
                {
                    res = new List<CurrentConditionsModel>() { };
                    var currentCondition = new CurrentConditionsModel()
                    {
                        LocalObservationDateTime = null,
                        EpochTime = 0,
                        WeatherText = null,
                        WeatherIcon = 0,
                        HasPrecipitation = false,
                        PrecipitationType = null,
                        IsDayTime = false,
                        Temperature = new _Temperature() { Metric = new _Unit(), Imperial = new _Unit() },
                        MobileLink = null,
                        Link = null
                    };

                    currentCondition.WeatherText = cityWeather.WeatherText;
                    currentCondition.Temperature.Metric.Value = cityWeather.CelsiusTemperature;
                    res.Add(currentCondition);
                }

            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("GetCurrentConditions error : ", ex.ToString(), EventLogEntryType.Error);
                return StatusCode(500); //Internal Server Error
            }
            return Ok(res);
        }

        [HttpPost("AddFavorite")]
        public IActionResult AddFavorite(Favorite request)
        {
            try
            {
                //Check if already exist
                var isCityKeyExist = db.Favorites.Where(s => s.CityKey == request.CityKey)
               .FirstOrDefault();

                if (isCityKeyExist == null)
                {
                    db.Favorites.Add(request);
                    db.SaveChanges();
                }
                else
                {
                    return Content("CityKey already exist !");
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("AddToFavorites error : ", ex.ToString(), EventLogEntryType.Error);
                return StatusCode(500); //Internal Server Error

            }

            return Ok(request);
        }

        //http://localhost:58365/weather/DeleteFavorite?cityKey=225007
        [HttpDelete("DeleteFavorite")]
        public IActionResult DeleteFavorite(string cityKey)
        {
             try
            {
                if (!(db.Favorites.Count() > 0)) return Content("Favorites list is empty !");

                var isCityKeyExist = db.Favorites.Where(s => s.CityKey == cityKey)
                .FirstOrDefault();


                if (isCityKeyExist != null)
                {
                    db.Favorites.Remove(db.Favorites.Single(a => a.CityKey == cityKey));
                    db.SaveChanges();
                }
                else
                    return Content("Data Not Found !");
             }
            catch (Exception ex)
            {
                EventLog.WriteEntry("DeleteFavorite error : ", ex.ToString(), EventLogEntryType.Error);
                return StatusCode(500); //Internal Server Error
            }
            return Content("CityKey " + cityKey + " deleted!");
        }


    }
}

