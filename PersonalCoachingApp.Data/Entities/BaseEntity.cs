using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace PersonalCoachingApp.Data.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }

        // Eğer DateTime kullanacaksan UTC olarak ayarla
        public DateTime CreatedDate { get; set; }  // UTC olarak başlatılacak.
        public DateTime? ModifiedDate { get; set; } // UTC olarak başlatılacak.

        public bool IsDeleted { get; set; }

    }

    // BaseConfiguration sınıfı, BaseEntity'den türeyen tüm entity'lere konfigürasyon eklemek için kullanılır
    public abstract class BaseConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasQueryFilter(x => x.IsDeleted == false);
            // Filtering that will be valid in this database and LINQs.

            builder.Property(x => x.ModifiedDate).IsRequired(false);
        }
    }
}
