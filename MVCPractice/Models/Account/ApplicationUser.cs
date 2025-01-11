using Microsoft.AspNetCore.Identity;
using MVCPractice.Models.Activities;
using System.ComponentModel.DataAnnotations;

namespace MVCPractice.Models.Account
{
    public partial class ApplicationUser : IdentityUser
    {
        [MaxLength(20)]
        public string Name { get; set; } = null!;

        public DateTime? Birthday { get; set; }

        [MaxLength(20)]
        public string Gender { get; set; }

        [MaxLength(20)]
        public string IdNumber { get; set; } = null!;

        [MaxLength(30)]
        public string Profession { get; set; }

        [MaxLength(20)]
        public string Education { get; set; }

        [MaxLength(20)]
        public string Marriage { get; set; }

        [MaxLength(20)]
        public string Religion { get; set; }

        [MaxLength(20)]
        public string LandlinePhone { get; set; }

        [MaxLength(100)]
        public string Address { get; set; }

        public virtual ICollection<ActivityData> UserCreatedActivityDatas { get; set; }
        public virtual ICollection<ActivityData> UserUpdateActivityDatas { get; set; }
        public virtual ICollection<ParticipatedActivityRecord> UserCreatedParticipatedActivityRecords { get; set; }
    }
}