using System.ComponentModel.DataAnnotations;

namespace HotelListing.API.Models.User
{
    public class LoginDto
    {

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(15, ErrorMessage = "Your Password is limited to 6 to 15 characters", MinimumLength = 6)]
        public string Password { get; set; }
    }
}
