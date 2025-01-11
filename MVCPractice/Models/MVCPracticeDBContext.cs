using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MVCPractice.Models.Account;
using MVCPractice.Models.Activities;

namespace MVCPractice.Models
{
    public class MVCPracticeDBContext : IdentityDbContext<ApplicationUser>
    {
        public MVCPracticeDBContext(DbContextOptions<MVCPracticeDBContext> options)
    : base(options)
        {
        }

        public virtual DbSet<RegisterTerm> RegisterTerms { get; set; }
        public virtual DbSet<ActivityData> ActivityDatas { get; set; }
        public virtual DbSet<ActivityImage> ActivityImages { get; set; }
        public virtual DbSet<ActivityFile> ActivityFiles { get; set; }
        public virtual DbSet<ActivityCategory> ActivityCategorys { get; set; }
        public virtual DbSet<ParticipatedActivityRecord> ParticipatedActivityRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActivityData>(entity =>
            {
                entity.Property(x => x.Id).HasDefaultValueSql("(newid())");
                entity.Property(x => x.Name).IsRequired();
                entity.Property(x => x.OrderIndex).HasDefaultValue(0);
                entity.Property(x => x.CategoryId).IsRequired();
                entity.Property(x => x.PersonsNumber).IsRequired();
                entity.Property(x => x.Introduce).HasMaxLength(250);
                entity.Property(x => x.Content).IsRequired();
                entity.Property(x => x.Enabled).HasDefaultValue(false);
                entity.Property(x => x.RegistrationStartDateTime).HasDefaultValueSql("(getdate())").HasColumnType("datetime");
                entity.Property(x => x.RegistrationEndDateTime).HasDefaultValueSql("(getdate())").HasColumnType("datetime");
                entity.Property(x => x.StartDateTime).HasDefaultValueSql("(getdate())").HasColumnType("datetime");
                entity.Property(x => x.EndDateTime).HasDefaultValueSql("(getdate())").HasColumnType("datetime");
                entity.Property(x => x.CreatedDateTime).HasDefaultValueSql("(getdate())").HasColumnType("datetime");
                entity.Property(x => x.CreatedUserId).IsRequired();
                entity.Property(x => x.UpdatedDateTime).HasDefaultValueSql("(getdate())").HasColumnType("datetime");
                entity.Property(x => x.UpdatedUserId).IsRequired();

                entity.HasOne(d => d.ActivityCategory)
                    .WithMany(p => p.ActivityDatas)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.CreatedUser)
                    .WithMany(p => p.UserCreatedActivityDatas)
                    .HasForeignKey(d => d.CreatedUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.UpdateUser)
                    .WithMany(p => p.UserUpdateActivityDatas)
                    .HasForeignKey(d => d.UpdatedUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });
            modelBuilder.Entity<ActivityImage>(entity =>
            {
                entity.Property(x => x.Id).HasDefaultValueSql("(newid())");
                entity.Property(x => x.ActivityId).IsRequired();
                entity.Property(x => x.Name).IsRequired();
                entity.Property(x => x.IsCover).HasDefaultValue(false);
                entity.Property(x => x.OrderIndex).HasDefaultValue(0);
                entity.Property(x => x.Url).IsRequired();

                entity.HasOne(d => d.ActivityData)
                    .WithMany(p => p.ActivityImages)
                    .HasForeignKey(d => d.ActivityId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });
            modelBuilder.Entity<ActivityFile>(entity =>
            {
                entity.Property(x => x.Id).HasDefaultValueSql("(newid())");
                entity.Property(x => x.ActivityId).IsRequired();
                entity.Property(x => x.Name).IsRequired();
                entity.Property(x => x.OrderIndex).HasDefaultValue(0);
                entity.Property(x => x.Url).IsRequired();

                entity.HasOne(d => d.ActivityData)
                    .WithMany(p => p.ActivityFiles)
                    .HasForeignKey(d => d.ActivityId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });
            modelBuilder.Entity<ActivityCategory>(entity =>
            {
                entity.Property(x => x.Id).HasDefaultValueSql("(newid())");
                entity.Property(x => x.OrderIndex).HasDefaultValue(0);
                entity.Property(x => x.Name).IsRequired();
            });
            modelBuilder.Entity<ParticipatedActivityRecord>(entity =>
            {
                entity.Property(x => x.Id).HasDefaultValueSql("(newid())");
                entity.Property(x => x.ActivityId).IsRequired();
                entity.Property(x => x.PersonsNumber).IsRequired();
                entity.Property(x => x.IsCancel).HasDefaultValue(false);
                entity.Property(x => x.CreatedDateTime).HasDefaultValueSql("(getdate())").HasColumnType("datetime");
                entity.Property(x => x.CreatedUserId).IsRequired();

                entity.HasOne(d => d.ActivityData)
                    .WithMany(p => p.ParticipatedActivityRecords)
                    .HasForeignKey(d => d.ActivityId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
                entity.HasOne(d => d.CreatedUser)
                    .WithMany(p => p.UserCreatedParticipatedActivityRecords)
                    .HasForeignKey(d => d.CreatedUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            base.OnModelCreating(modelBuilder);
        }

        /*
        Add-Migration <MigrationName> -Context <ContextName>  //建立遷移 <MigrationName>  每次都需不同
        Update-Database -Context WebContext //更新資料庫
        */
    }
}