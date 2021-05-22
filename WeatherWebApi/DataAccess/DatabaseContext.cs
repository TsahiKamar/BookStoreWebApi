
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WeatherApi.DataAccess.Entities;

namespace WeatherApi.Dal
{
    public class DatabaseContext : DbContext
    {
        public DbSet<CityWeather> CitiesWeather { get; set; }

        public DbSet<Favorite> Favorites { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuider)
        {
            optionsBuider.UseSqlServer(@"Server=DESKTOP-TRIEUB2;Database=WeatherDB;Trusted_Connection=True;",
            options => options.EnableRetryOnFailure());
        }
    }
}

