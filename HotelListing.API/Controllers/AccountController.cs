using HotelListing.API.Core.Contract;
using HotelListing.API.Core.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthManagerService _authManagerService;
        private readonly ILogger<AccountController> _logger;
        public AccountController(IAuthManagerService authManagerService, ILogger<AccountController> logger)
        {
            _authManagerService = authManagerService;
            _logger = logger;

        }
        // POST: api/Account/register
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<ActionResult> Register([FromBody]ApiUserDto aPiUserDto)
        {
            try
            {
                _logger.LogInformation($"Registeration attempt for {aPiUserDto.Email}");
                IEnumerable<IdentityError> errors = await _authManagerService.RegisterAsync(aPiUserDto);

                if (errors.Any())
                {
                    foreach (var error in errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    _logger.LogWarning($"Something went wrong while registering user {ModelState.ToString()}");
                    return BadRequest(ModelState);

                }
                return Ok();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(Register)} - User Registeration attempt for {aPiUserDto.Email}");

                return Problem($"Something went wrong in the {nameof(Register)}. Please contact support.", statusCode: 500);
    
            }
          
           
        }


        // POST: api/Account/login
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<ActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                _logger.LogInformation($"Login attempt by {loginDto.Email}");
                var authResponse = await _authManagerService.LoginAsync(loginDto);

                if (authResponse is not null)
                    return Ok(authResponse);

                return Unauthorized();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"something went wrong in {nameof(Login)} while user {loginDto.Email} tried to log");

                return Problem($"something went wrong in {nameof(Login)}", statusCode: 500);
                
            }

           

        }

        // POST: api/Account/refreshtoken
        [HttpPost]
        [Route("refreshtoken")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<ActionResult> RefreshTokenAsync([FromBody] AuthResponseDto request)
        {
            var authResponse = await _authManagerService.VerifyRefreshToken(request);

            if(authResponse is null)
            {
                return Unauthorized();
            }
            return Ok(authResponse);

        }



    }
}
