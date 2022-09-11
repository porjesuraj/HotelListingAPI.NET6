using HotelListing.API.Contract;
using HotelListing.API.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthManagerService _authManagerService;
        public AccountController(IAuthManagerService authManagerService)
        {
            _authManagerService = authManagerService;

        }
        // POST: api/Account/register
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<ActionResult> Register([FromBody]ApiUserDto aPiUserDto)
        {
            IEnumerable<IdentityError> errors = await _authManagerService.RegisterAsync(aPiUserDto);

            if(errors.Any())
            {
                foreach (var error in errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);

            }
            return Ok();
           
        }


        // POST: api/Account/login
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<ActionResult> Login([FromBody] LoginDto loginDto)
        {

            var isValidUser = await _authManagerService.LoginAsync(loginDto);

            if(isValidUser)
                return Ok();

            return Unauthorized();

        }



    }
}
