using HotelListing.API.Contract;
using HotelListing.API.Data;

namespace HotelListing.API.Repository
{
    public class CountriesRepository: GenericRepository<Country>,ICountriesRepository
    {
        public CountriesRepository(HotelListingDbContext dbContext):base(dbContext)
        {

        }
    }
}
