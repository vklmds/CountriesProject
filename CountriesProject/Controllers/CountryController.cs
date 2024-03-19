using CountriesProject.Services;
using Microsoft.AspNetCore.Mvc;


namespace CountriesProject.Controllers
{
    [ApiController]
    [Route("api/controller")]
    public class CountryController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private const string RestCountriesApiUrl = "https://restcountries.com/v2/all";
        private const string cacheKey = "TestKey";
        private readonly CountryService _countryService;       

        public CountryController(CountryService countryService, IHttpClientFactory clientFactory, ExceptionHandlingMiddleware exceptionHandlingMiddleware )
        {
            _countryService = countryService;
            _clientFactory = clientFactory;           
        }

        [HttpGet]
        [Route("getCountries")]
        public async Task<IActionResult> GetCountries()
        {            
           
            if (_countryService.IsCacheFull(cacheKey))
            {
                var listInCache = _countryService.GetFromCache(cacheKey);
                return Ok(listInCache);
            }
            else
            {
                var dbList = await _countryService.GetAllCountries();
                if (dbList.Any())
                {
                    _countryService.SavedInCache(cacheKey, dbList);
                    return Ok(dbList);
                }
                else
                {
                    var _httpClient = _clientFactory.CreateClient();
                    var countries = await _countryService.GetCountries(_httpClient, RestCountriesApiUrl);
                    await _countryService.SaveCountries(countries);
                    _countryService.SavedInCache(cacheKey, countries);
                    return Ok(countries);
                }
            }
         }
        
        
        
        [HttpPost]
        [Route("getCountryInfo")]
        public async Task<IActionResult> GetCountryInfo([FromQuery] string name)
        {
            var _httpClient = _clientFactory.CreateClient();
            var country = await _countryService.GetCountry(_httpClient, $"https://restcountries.com/v2/name/{name}");         
            return Ok(country);            
        }
        
    }
}
