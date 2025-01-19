using System.ComponentModel.DataAnnotations;

namespace PersonalCoaching.WebApi.Models
{
    public class AddFeatureRequest
    {
        [Required(ErrorMessage = "This field is required.")]
        public string Title { get; set; }
    }
}
