using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MemoryCacheDemo.InMemoryCache;
using Microsoft.AspNetCore.Mvc;

namespace MemoryCacheDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InMemoryCacheController(LocationRepository repository) : ControllerBase
    {
        private readonly LocationRepository _repository = repository;

        // Lấy toàn bộ danh sách quốc gia
        // GET: api/location/countries
        [HttpGet("countries")]
        public async Task<IActionResult> GetCountries()
        {
            var countries = await _repository.GetCountriesAsync();
            return Ok(countries);
        }

        // Lấy danh sách bang/tỉnh theo countryId
        // GET: api/location/states/{countryId}
        [HttpGet("states/{countryId}")]
        public async Task<IActionResult> GetStates(int countryId)
        {
            var states = await _repository.GetStatesAsync(countryId);
            return Ok(states);
        }

        // Lấy danh sách thành phố theo stateId
        // GET: api/location/cities/{stateId}
        [HttpGet("cities/{stateId}")]
        public async Task<IActionResult> GetCities(int stateId)
        {
            var cities = await _repository.GetCitiesAsync(stateId);
            return Ok(cities);
        }   
    }
}