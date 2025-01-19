using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonalCoachingApp.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalCoachingApp.Data.Entities
{
    public class PackageEntity : BaseEntity
    {

        public string PackageName { get; set; }

        public decimal Price { get; set; }

        public int StockQuantity { get; set; }

        public int TrainingDuration { get; set; }

        public PackageType PackageType { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public string? Description { get; set; }

        //Relational Property

        public ICollection<PackageFeatureEntity> PackageFeatures { get; set; }

    }

    public class PackageConfiguration : BaseConfiguration<PackageEntity>
    {
        public override void Configure(EntityTypeBuilder<PackageEntity> builder)
        {
            builder.Property(x => x.PackageName)
                .IsRequired()
                .HasMaxLength(100);

            base.Configure(builder);
        }
    }
}
