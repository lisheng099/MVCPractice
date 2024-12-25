using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MVCPractice.Areas.Identity.Data;
using MVCPractice.Models.Account;
using MVCPractice.Models.Activities;

namespace MVCPractice.Models
{
    public class MVCPracticeDBContext : IdentityDbContext<MVCPracticeUser>
    {
        /*
        
        Add-Migration <MigrationName> -Context <ContextName>  //建立遷移 <MigrationName>  每次都需不同
        Update-Database -Context MVCPracticeDBContext //更新資料庫
        */
        public MVCPracticeDBContext(DbContextOptions<MVCPracticeDBContext> options)
    : base(options)
        {
        }

        public DbSet<MVCPracticeUser> ApplicationUsers { get; set; }

        public virtual DbSet<RegisterTerm> RegisterTerms { get; set; }
        public virtual DbSet<ActivityData> ActivityDatas { get; set; }
        public virtual DbSet<ActivityImage> ActivityImages { get; set; }
        public virtual DbSet<ActivityFile> ActivityFiles { get; set; }
        public virtual DbSet<ActivityCategory> ActivityCategorys { get; set; }
        public virtual DbSet<ParticipatedActivityRecord> ParticipatedActivityRecords { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<RegisterTerm>().ToTable("RegisterTermsList");
            builder.Entity<ActivityData>().ToTable("ActivityDatas");
            builder.Entity<ActivityImage>().ToTable("ActivityImages");
            builder.Entity<ActivityFile>().ToTable("ActivityFiles");
            builder.Entity<ActivityCategory>().ToTable("ActivityCategory");
            builder.Entity<ParticipatedActivityRecord>().ToTable("ParticipatedActivityRecord");
            base.OnModelCreating(builder);
        }
    }
}
