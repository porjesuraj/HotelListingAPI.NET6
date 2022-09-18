using AutoMapper;
using AutoMapper.QueryableExtensions;
using HotelListing.API.Core.Contract;
using HotelListing.API.Core.Exceptions;
using HotelListing.API.Core.Models.Country;
using HotelListing.API.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Core.Repository
{
    public class CountriesRepository: GenericRepository<Country>,ICountriesRepository
    {
        private readonly HotelListingDbContext _hotelListingDbContext;
        private readonly IMapper _mapper;
        public CountriesRepository(HotelListingDbContext dbContext, IMapper mapper):base(dbContext,mapper)
        {
            _hotelListingDbContext = dbContext;
            _mapper = mapper;
        }

        public Task<Country> GetDetails(int? id)
        {
            return _hotelListingDbContext.Countries.Include(q => q.Hotels).FirstOrDefaultAsync(q => q.Id == id);

        }

        public async Task<CountryDetailsDto> GetDetailsAync(int? id)
        {
            var country = await _hotelListingDbContext.Countries.Include(q => q.Hotels)
                .ProjectTo<CountryDetailsDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (country == null)
                throw new NotFoundException(nameof(GetDetailsAync), id.HasValue ? id: "No Key Provided");

            return country;
        }
    }
}
