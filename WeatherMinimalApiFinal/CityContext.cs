using Microsoft.EntityFrameworkCore;

namespace WeatherMinimalApiFinal
{

    // need 2st nuget package: Microsoft.AspNetCore.Diagonostics.EntityFrameworkCore
    // Microsoft.EntityFrameworkcore.InMemory
    public class CityContext : DbContext
    {
        public CityContext(DbContextOptions<CityContext> options) : base(options)
        {

        }
        public DbSet<City> Cities { get; set; }
    }
}
