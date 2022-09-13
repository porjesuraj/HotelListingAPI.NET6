using AutoMapper;
using HotelListing.API.Contract;
using HotelListing.API.Data;
using HotelListing.API.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HotelListing.API.Services
{
    public class AutoManagerService : IAuthManagerService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApiUser> _userManager;
        private readonly IConfiguration _configuration;
        public AutoManagerService(IMapper mapper, UserManager<ApiUser> userManager, IConfiguration configuration)
        {
            _mapper = mapper;
            _userManager = userManager;
            _configuration = configuration;

        }

        public async Task<string> GenerateToken(ApiUser apiUser)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var roles = await _userManager.GetRolesAsync(apiUser);
            var rolesClaim = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();
            var userClaims = await _userManager.GetClaimsAsync(apiUser);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, apiUser.Email ),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString() ),
                new Claim(JwtRegisteredClaimNames.Email, apiUser.Email),
                new Claim("uid", apiUser.Id)
            }
            .Union(userClaims).Union(rolesClaim);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["JwtSettings:DurationInMinutes"])),
                signingCredentials: credentials);

                return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            bool isValidUser = false;
            try
            {
                var user = await _userManager.FindByEmailAsync(loginDto.Email);
             //   isValidUser = await _userManager.CheckPasswordAsync(user, loginDto.Password);
                if (user == null )
                    return null;

                var token = await GenerateToken(user);

                return new AuthResponseDto
                {
                    Token = token,
                    UserId = user.Id
                };
            }
            catch (Exception)
            {
                return null;
            }
            


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
