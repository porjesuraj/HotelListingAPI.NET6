using AutoMapper;
using HotelListing.API.Core.Contract;
using HotelListing.API.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Core.Repository
{
    public class CountriesRepository: GenericRepository<Country>,ICountriesRepository
    {
        private readonly HotelListingDbContext _hotelListingDbContext;
        public CountriesRepository(HotelListingDbContext dbContext, IMapper mapper):base(dbContext,mapper)
        {
            _hotelListingDbContext = dbContext;
        }

        public Task<Country> GetDetails(int? id)
        {
            return _hotelListingDbContext.Countries.Include(q => q.Hotels).FirstOrDefaultAsync(q => q.Id == id);

        }
    }
}
