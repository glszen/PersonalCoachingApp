using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonalCoachingApp.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalCoachingApp.Data.Entities
{
    public class UserEntity : BaseEntity
    {
        public string Email { get; set; }

        [DataType(DataType.Password)] //Hidden password.
        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime BirthDate { get; set; }

        public UserType UserType { get; set; }

    }

    public class UserConfiguration : BaseConfiguration<UserEntity>
    {
        public override void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.Property(x => x.FirstName).IsRequired().HasMaxLength(40);
            builder.Property(x => x.LastName).IsRequired().HasMaxLength(40);

            base.Configure(builder);
        }
    }
}
