using System.ComponentModel.DataAnnotations;

namespace PersonalCoaching.WebApi.Models
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "This field is required.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [DataType(DataType.Password)] //Hidden password.
        public string Password { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public string FirstName {  get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public DateTime BirthDate {  get; set; }
    }
}
