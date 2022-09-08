using HotelListing.API.Data;

namespace HotelListing.API.Contract
{
    public interface ICountriesRepository : IGenericRepository<Country>
    {
        Task<Country> GetDetails(int? id);
    }
}
