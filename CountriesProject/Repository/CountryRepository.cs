using CountriesProject.Controllers;
using CountriesProject.Interfaces;
using CountriesProject.Models;
using Microsoft.EntityFrameworkCore;

namespace CountriesProject.Repository
{
    public class CountryRepository : ICountryRepository
    {
        private readonly ApplicationDbContext _con; 


        public CountryRepository(ApplicationDbContext con, ILogger<CountryRepository> logger)
        {
            _con = con;      
        }
        public async Task SaveCountries(List<Country> countries)
        {         
          
            foreach (var country in countries)
            {
                await _con.Database.ExecuteSqlAsync(
                    $"EXEC SaveCountries @Name={country.name}, @Capital={country.capital}, @Borders={string.Join(',', country.borders)}");
            }
        }

        public async Task<List<Country>> GetAllCountries()
        {
            var listOfCountries = new List<Country>();           
      
            listOfCountries = await _con.CountriesDBO
                .FromSqlRaw("SELECT * FROM Countries")
                .Select(c => new Country
                {
                    id = c.id,
                    name = c.name,
                    capital = c.capital,
                    borders = c.borders.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToArray()
                })
                .ToListAsync();

            return listOfCountries;
        }
    }
}
