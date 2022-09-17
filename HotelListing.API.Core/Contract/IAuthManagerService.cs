using HotelListing.API.Data;
using HotelListing.API.Core.Models.User;
using Microsoft.AspNetCore.Identity;

namespace HotelListing.API.Core.Contract
{
    public interface IAuthManagerService
    {
        Task<IEnumerable<IdentityError>> RegisterAsync(ApiUserDto userDto);
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        Task<string> GenerateToken(ApiUser apiUser);

        Task<string> CreateRefreshTokenAsync();

        Task<AuthResponseDto> VerifyRefreshToken(AuthResponseDto request);

    }
}
