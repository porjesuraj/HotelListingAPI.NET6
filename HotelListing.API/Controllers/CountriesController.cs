using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelListing.API.Data;
using HotelListing.API.Models.Country;
using HotelListing.API.Configuration;
using AutoMapper;
using HotelListing.API.Contract;
using Microsoft.AspNetCore.Authorization;
using HotelListing.API.Exceptions;
using HotelListing.API.Models.Query;

namespace HotelListing.API.Controllers
{
    [Route("api/v{version:apiVersion}/countries")]
    [ApiController]
    [ApiVersion("1.0",Deprecated =true)]
    public class CountriesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICountriesRepository _countriesRepository;
        public CountriesController(ICountriesRepository countriesRepository, IMapper mapper)
        {
            _countriesRepository = countriesRepository;
            _mapper = mapper;
        }

        // GET: api/Countries
        [HttpGet]      
        public async Task<ActionResult<IEnumerable<GetCountryDto>>> GetCountries()
        {
            if (!_countriesRepository.TableExist())
            {
                return NotFound();
            }
     
            var countries = await _countriesRepository.GetAllAsync();

            if (countries == null)
            {
                throw new NotFoundException(nameof(GetCountries), countries.Count.ToString());
            }

            var result =  _mapper.Map<List<GetCountryDto>>(countries);
            return Ok(result);
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
            var country = await _countriesRepository.GetDetails(id);
            //    _context.Countries.Include(q => q.Hotels)   .FirstOrDefaultAsync(q=> q.Id == id);



            if (country == null)
            {
                throw new NotFoundException(nameof(GetCountry), id.ToString());
            }
            var countryDto = _mapper.Map<CountryDetailsDto>(country);

            return Ok(countryDto);
        }

        // PUT: api/Countries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = Constants.StringConstants.ADMIN_ROLE)]
        [HttpPut("{id}")]      
        public async Task<IActionResult> PutCountry(int id, UpdateCountryDto updateCountryDto)
        {
            if (id != updateCountryDto.Id)
            {
                return BadRequest();
            }

            /// _context.Entry(country).State = EntityState.Modified;
            /// 
            var country = await _countriesRepository.GetAsync(id);

            if(country == null)
            {
                throw new NotFoundException(nameof(PutCountry), id.ToString());
            }
            _mapper.Map(updateCountryDto, country);

          await  _countriesRepository.UpdateAsync(country);
           

            return NoContent();
        }

        // POST: api/Countries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = Constants.StringConstants.ADMIN_ROLE)]
        [HttpPost]
        public async Task<ActionResult<Country>> PostCountry(CreateCountryDto createCountryDto)
        {
            // var country = new Country { Name = createCountryDto.Name, ShortName = createCountryDto.ShortName };
            var country = _mapper.Map<Country>(createCountryDto);
          if (!_countriesRepository.TableExist())
          {
              return Problem("Entity set 'HotelListingDbContext.Countries'  is null.");
          }
           var result = await  _countriesRepository.AddAsync(country);

            return CreatedAtAction("GetCountry", new { id = result.Id }, result);
        }

        // DELETE: api/Countries/5
        [Authorize(Roles = Constants.StringConstants.ADMIN_ROLE)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            if (!_countriesRepository.TableExist())
            {
                return NotFound();
            }
            var country = await _countriesRepository.GetAsync(id);
            if (country == null)
            {
                throw new NotFoundException(nameof(DeleteCountry), id.ToString());
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
