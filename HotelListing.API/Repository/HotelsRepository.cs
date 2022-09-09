﻿using HotelListing.API.Contract;
using HotelListing.API.Data;

namespace HotelListing.API.Repository
{
    public class HotelsRepository : GenericRepository<Hotel>, IHotelsRepository
    {
        public HotelsRepository(HotelListingDbContext hotelListingDbContext) : base(hotelListingDbContext)
        {
        }
    }
}
