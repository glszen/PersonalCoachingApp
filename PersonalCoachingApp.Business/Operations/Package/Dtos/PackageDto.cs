using PersonalCoachingApp.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalCoachingApp.Business.Operations.Package.Dtos
{
    public class PackageDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal PackagePrice { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public string? Description { get; set; }

        public int TrainingDuration { get; set; }

        public PackageType PackageType { get; set; }

        public int StockQuantity { get; set; }

        public List<PackageFeatureDto> Features { get; set; }
    }
}
