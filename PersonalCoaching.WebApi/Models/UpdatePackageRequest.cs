using PersonalCoachingApp.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace PersonalCoaching.WebApi.Models
{
    public class UpdatePackageRequest
    {
        [Required(ErrorMessage = "This field is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public int PackagePrice { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public int TrainingDuration { get; set; }

        public PackageType PackageType { get; set; }

        public int StockQuantity { get; set; }

        public List<int> FeatureIds { get; set; }
    }
}
