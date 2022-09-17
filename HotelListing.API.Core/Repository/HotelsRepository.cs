using AutoMapper;
using HotelListing.API.Core.Contract;
using HotelListing.API.Data;

namespace HotelListing.API.Core.Repository
{
    public class HotelsRepository : GenericRepository<Hotel>, IHotelsRepository
    {
        public HotelsRepository(HotelListingDbContext hotelListingDbContext, IMapper mappper) : base(hotelListingDbContext, mappper)
        {
        }
    }
}
