using CountriesProject.Interfaces;
using CountriesProject.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace CountriesProject.Services
{
    public class CountryService
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IMemoryCache _memoryCache;
        

        public CountryService(ICountryRepository countryRepository, IMemoryCache memoryCache)
        {
            _countryRepository = countryRepository;
            _memoryCache = memoryCache;
        }

        public async Task<List<Country>> GetCountries (HttpClient httpClient, string RestCountriesApiUrl)
        {

            List<Country> formattedCountries = new List<Country>();

            HttpResponseMessage response = await httpClient.GetAsync(RestCountriesApiUrl);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();               
            List<Country> countries = JsonSerializer.Deserialize<List<Country>>(responseBody);

            foreach (var country in countries)
            {
                formattedCountries.Add(new Country
                {
                    name = country.name,
                    capital = country.capital?.ToString() ?? "",
                    borders = country.borders ?? new string[0]
                });
            }            
            return formattedCountries; 
        }

        public async Task<CountryInfo> GetCountry(HttpClient httpClient, string RestCountryApiUrl)
        {
            var response = await httpClient.GetAsync(RestCountryApiUrl);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var content = await response.Content.ReadAsStringAsync();
            //var countryInfo = JsonSerializer.Deserialize<CountryInfo>(content);                                         

            var countryResponse = new CountryInfo
            {
                name = "United States",
                region = "region",
                subregion = "subregion",
                population = 100000

            };
            return countryResponse;
        }

        public async Task SaveCountries(List<Country> countries)
        {
            await _countryRepository.SaveCountries(countries);

        }

        public async Task<List<Country>> GetAllCountries()
        {
            var listOfCountries = await _countryRepository.GetAllCountries();
            return listOfCountries;
        }
        
        public bool IsCacheFull(string cacheKey)
        {
            return (_memoryCache.TryGetValue(cacheKey, out List<Country> countries));
                                
        }


        public List<Country> GetFromCache(string cacheKey)
        {
            return _memoryCache.TryGetValue(cacheKey ,out List<Country> countries) ? countries : new List<Country>();           
        }

        public void SavedInCache(string cacheKey, List<Country> countries)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(5));            
            _memoryCache.Set(cacheKey, countries, cacheEntryOptions);
        }

    }

}
