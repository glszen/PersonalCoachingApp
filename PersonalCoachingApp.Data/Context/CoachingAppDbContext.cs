using Microsoft.EntityFrameworkCore;
using PersonalCoachingApp.Data.Entities;

namespace PersonalCoachingApp.Data.Context
{
    public class CoachingAppDbContext : DbContext
    {
        public CoachingAppDbContext(DbContextOptions<CoachingAppDbContext> options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Fluent API konfigürasyonlarını uyguluyoruz
            modelBuilder.ApplyConfiguration(new FeatureConfiguration());
            modelBuilder.ApplyConfiguration(new PackageConfiguration());
            modelBuilder.ApplyConfiguration(new PackageFeatureConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());

            modelBuilder.Entity<SettingEntity>().HasData(
                new SettingEntity
                {
                    Id = 1,
                    MaintencenceMode = false
                });

            base.OnModelCreating(modelBuilder);
        }

        // DbSet'ler
        public DbSet<UserEntity> Users => Set<UserEntity>();
        public DbSet<FeatureEntity> Features => Set<FeatureEntity>();
        public DbSet<PackageEntity> Packages => Set<PackageEntity>();
        public DbSet<PackageFeatureEntity> PackageFeatureEntities => Set<PackageFeatureEntity>();
        public DbSet<SettingEntity> Settings {  get; set; }
    }
}
