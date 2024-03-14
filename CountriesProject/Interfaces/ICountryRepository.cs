using CountriesProject.Models;

namespace CountriesProject.Interfaces
{
    public interface ICountryRepository
    { 
        Task SaveCountries(List<Country> country);
        Task<List<Country>> GetAllCountries();
    }
}
