using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalCoachingApp.Data.Entities
{
    //Which features are included in which package?
    public class PackageFeatureEntity : BaseEntity
    {
        public int PackageId { get; set; }
        public int FeatureId { get; set; }


        //Relational Property

        public PackageEntity Package { get; set; }

        public FeatureEntity Feature { get; set; }

    }

    public class PackageFeatureConfiguration : BaseConfiguration<PackageFeatureEntity>
    {
        public override void Configure(EntityTypeBuilder<PackageFeatureEntity> builder)
        {
            builder.Ignore(x => x.Id);
            builder.HasKey("PackageId", "FeatureId");

            base.Configure(builder);
        }
    }
}

