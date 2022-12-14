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
using HotelListing.API.Core.Models.Country;
using HotelListing.API.Core.Models.Query;
using HotelListing.API.Core.Models.Hotel;
using HotelListing.API.Core.Contract;

namespace HotelListing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        #region Private Variable
        private readonly IHotelsRepository _context;
        #endregion

        #region Constructor
        public HotelsController(IHotelsRepository context, IMapper mapper)
        {
            _context = context;
        }
        #endregion


        // GET: api/Hotels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HotelDto>>> GetHotels()
        {
            if(!_context.TableExist())
            {
                return NotFound();
            }

            var hotels = await _context.GetAllAsync<HotelDto>();
            return Ok(hotels);

        }

        // GET: api/Hotels
        [HttpGet("GetPaged")]
        public async Task<ActionResult<PageResult<HotelDto>>> GetPagedHotels( [FromQuery]QueryParameters queryParameters)
        {
            if (!_context.TableExist())
            {
                return NotFound();
            }

            var pageResult = await _context.GetAllAsync<HotelDto>(queryParameters);
   
            return Ok(pageResult);

        }

        // GET: api/Hotels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetHotelDetailsDto>> GetHotel(int id)
        {
            if (!_context.TableExist())
            {
                return NotFound();
            }
            var hotel = await _context.GetAsync<GetHotelDetailsDto>(id) ;



            return Ok(hotel);
        }

        // PUT: api/Hotels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHotel(int id, HotelDto hotel)
        {
            if (!_context.TableExist())
            {
                return NotFound();
            }

            try
            {


                await _context.UpdateAsync(id,hotel);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (! await HotelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Hotels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CreateHotelDto>> PostHotel(CreateHotelDto hotelDto)
        {
            if (!_context.TableExist())
            {
                return NotFound();
            }
          var record = await _context.AddAsync<CreateHotelDto, HotelDto>(hotelDto);

            return CreatedAtAction("GetHotel", new { id = record.Id }, record);
        }

        // DELETE: api/Hotels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            if (!_context.TableExist())
            {
                return NotFound();
            }
          await  _context.DeleteAsync(id);
            return NoContent();
        }

        private async Task<bool> HotelExists(int id)
        {
            return (await _context.GetAsync(id)) != null;
        }
    }
}
