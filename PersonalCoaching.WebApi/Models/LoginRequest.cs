using System.ComponentModel.DataAnnotations;

namespace PersonalCoaching.WebApi.Models
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "This field is required.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [DataType(DataType.Password)] //Hidden password.
        public string Password { get; set; }
    }
}
