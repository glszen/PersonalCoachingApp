using PersonalCoachingApp.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalCoachingApp.Business.Operations.Package.Dtos
{
    public class AddPackageDto
    {
        [Required(ErrorMessage = "This field is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public decimal PackagePrice { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public int TrainingDuration { get; set; }

        public PackageType PackageType { get; set; }

        public List<int> FeatureIds { get; set; }

        public int StockQuantity { get; set; }
    }
}
