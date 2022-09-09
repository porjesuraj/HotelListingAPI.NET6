﻿using HotelListing.API.Models.Country;

namespace HotelListing.API.Models.Hotel
{
    public class GetHotelDetailsDto : BaseHotelDto
    {
        public int Id { get; set; }

        public GetCountryDto Country { get; set; }
    }
}
