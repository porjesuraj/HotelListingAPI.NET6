using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelListing.API.Data;
using AutoMapper;

using Microsoft.AspNetCore.Authorization;

using HotelListing.API.Core.Contract;
using HotelListing.API.Core.Models.Country;
using HotelListing.API.Core.Exceptions;
using HotelListing.API.Core.Models.Query;

namespace HotelListing.API.Controllers
{
    [Route("api/v{version:apiVersion}/countries")]
    [ApiController]
    [ApiVersion("1.0",Deprecated =true)]
    public class CountriesController : ControllerBase
    {

        private readonly ICountriesRepository _countriesRepository;
        public CountriesController(ICountriesRepository countriesRepository, IMapper mapper)
        {
            _countriesRepository = countriesRepository;
        }

        // GET: api/Countries
        [HttpGet]      
        public async Task<ActionResult<IEnumerable<GetCountryDto>>> GetCountries()
        {

            if (!_countriesRepository.TableExist())
            {
                return NotFound();
            }
     
            var countries = await _countriesRepository.GetAllAsync<GetCountryDto>();

            if (countries == null)
            {
                throw new NotFoundException(nameof(GetCountries), countries.Count.ToString());
            }       
            return Ok(countries);
        }

        // GET: api/Countries/GetPaged/?StartIndex=0&PageSize=25&PageNumber=1
        [HttpGet("GetPaged")]
        public async Task<ActionResult<PageResult<GetCountryDto>>> GetPagedCountries([FromQuery] QueryParameters queryParameters)
        {
          

            var pageResult = await _countriesRepository.GetAllAsync<GetCountryDto>(queryParameters);

       
            return Ok(pageResult);
        }


        // GET: api/Countries/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<CountryDetailsDto>> GetCountry(int id)
        {
            if (!_countriesRepository.TableExist())
            {
                return NotFound();
            }
            var country = await _countriesRepository.GetDetailsAync(id);

            return Ok(country);
        }

        // PUT: api/Countries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = Core.Constants.StringConstants.ADMIN_ROLE)]
        [HttpPut("{id}")]      
        public async Task<IActionResult> PutCountry(int id, UpdateCountryDto updateCountryDto)
        {
            try
            {
                if (id != updateCountryDto.Id)
                {
                    return BadRequest();
                }
                await _countriesRepository.UpdateAsync(id, updateCountryDto);
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CountryExists(id))
                { return NotFound(); }
                else
                    throw;
            }
           
        }

        // POST: api/Countries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = Core.Constants.StringConstants.ADMIN_ROLE)]
        [HttpPost]
        public async Task<ActionResult<Country>> PostCountry(CreateCountryDto createCountryDto)
        {
          if (!_countriesRepository.TableExist())
          {
              return Problem("Entity set 'HotelListingDbContext.Countries'  is null.");
          }
           var result = await  _countriesRepository.AddAsync<CreateCountryDto, GetCountryDto>(createCountryDto);

            return CreatedAtAction("GetCountry", new { id = result.Id }, result);
        }

        // DELETE: api/Countries/5
        [Authorize(Roles = Core.Constants.StringConstants.ADMIN_ROLE)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            if (!_countriesRepository.TableExist())
            {
                return NotFound();
            }
           await _countriesRepository.DeleteAsync(id);

            return NoContent();
        }

        private bool CountryExists(int id)
        {
           var result = _countriesRepository.GetAsync(id);
            return result == null ? false : true;
        //    return (_context.Countries?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
