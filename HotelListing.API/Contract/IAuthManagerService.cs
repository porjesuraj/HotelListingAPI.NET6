using HotelListing.API.Models.User;
using Microsoft.AspNetCore.Identity;

namespace HotelListing.API.Contract
{
    public interface IAuthManagerService
    {
        Task<IEnumerable<IdentityError>> RegisterAsync(ApiUserDto userDto);
        Task<bool> LoginAsync(LoginDto loginDto);
    }
}
