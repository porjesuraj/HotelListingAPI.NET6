using HotelListing.API.Core.Models.Country;

namespace HotelListing.API.Core.Models.Hotel
{
    public class GetHotelDetailsDto : BaseHotelDto
    {
        public int Id { get; set; }

        public GetCountryDto Country { get; set; }
    }
}
