using AutoMapper;
using HotelListing.API.Constants;
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
        private ApiUser _user;
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
           // bool isValidUser = false;
            try
            {
                _user = await _userManager.FindByEmailAsync(loginDto.Email);
             //   isValidUser = await _userManager.CheckPasswordAsync(user, loginDto.Password);
                if (_user == null )
                    return null;

                var token = await GenerateToken(_user);

                return new AuthResponseDto
                {
                    Token = token,
                    UserId = _user.Id,
                    RefreshToken = await CreateRefreshTokenAsync()
                };
            }
            catch (Exception)
            {
                return null;
            }
            


        }

        public async Task<IEnumerable<IdentityError>> RegisterAsync(ApiUserDto userDto)
        {
            _user = _mapper.Map<ApiUser>(userDto);
            _user.UserName = userDto.Email;
            var result = await _userManager.CreateAsync(_user);

            if(result.Succeeded)
            {
                await _userManager.AddToRoleAsync(_user, "User");

            }

            return result.Errors;

        }

        public async Task<string> CreateRefreshTokenAsync()
        {
          await  _userManager.RemoveAuthenticationTokenAsync(_user, StringConstants.LOGIN_PROVIDER, StringConstants.REFRESH_TOKEN);
            var newRefreshToken = await _userManager.GenerateUserTokenAsync(_user, StringConstants.LOGIN_PROVIDER, StringConstants.REFRESH_TOKEN);

            var result = await _userManager.SetAuthenticationTokenAsync(_user, StringConstants.LOGIN_PROVIDER,StringConstants.REFRESH_TOKEN , newRefreshToken);
           
            return newRefreshToken;
        }

        public async Task<AuthResponseDto> VerifyRefreshToken(AuthResponseDto request)
        {
            _user = null;
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var tokenContent = jwtSecurityTokenHandler.ReadJwtToken(request.Token);

            var username = tokenContent.Claims.ToList().FirstOrDefault(q => q.Type == JwtRegisteredClaimNames.Email)?.Value;
            _user = await _userManager.FindByNameAsync(username);
            if (_user == null)
                return null;

            var isValidRefreshToken = await _userManager.VerifyUserTokenAsync(_user, StringConstants.LOGIN_PROVIDER,StringConstants.REFRESH_TOKEN,request.RefreshToken);

            if(isValidRefreshToken)
            {
                var token = await GenerateToken(_user);
                return new AuthResponseDto
                {
                    Token = token,
                    UserId = _user.Id,
                    RefreshToken = await CreateRefreshTokenAsync()
                };
               
               
            }
            else
            {
                await _userManager.UpdateSecurityStampAsync(_user);
                return null;
            }

          
         

        }
    }
}
