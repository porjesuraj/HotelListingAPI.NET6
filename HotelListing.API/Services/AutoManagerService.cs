using AutoMapper;
using HotelListing.API.Contract;
using HotelListing.API.Data;
using HotelListing.API.Models.User;
using Microsoft.AspNetCore.Identity;

namespace HotelListing.API.Services
{
    public class AutoManagerService : IAuthManagerService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApiUser> _userManager;
        public AutoManagerService(IMapper mapper, UserManager<ApiUser> userManager)
        {
            _mapper = mapper;
            _userManager = userManager;

        }

        public async Task<bool> LoginAsync(LoginDto loginDto)
        {
            bool isValidUser = false;
            try
            {
                var user = await _userManager.FindByEmailAsync(loginDto.Email);
                if (user is null)
                    return default;
                isValidUser = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            }
            catch (Exception)
            {

            }
            return isValidUser;


        }

        public async Task<IEnumerable<IdentityError>> RegisterAsync(ApiUserDto userDto)
        {
            var user = _mapper.Map<ApiUser>(userDto);
            user.UserName = userDto.Email;

            var result = await _userManager.CreateAsync(user);

            if(result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");

            }

            return result.Errors;

        }
    }
}
