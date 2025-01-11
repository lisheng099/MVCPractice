using MVCPractice.Models.Account;
using System.ComponentModel.DataAnnotations;

namespace MVCPractice.Models.Activities
{
    public partial class ActivityData
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;
        public int OrderIndex { get; set; }
        public Guid CategoryId { get; set; }
        public int PersonsNumber { get; set; }
        public string Introduce { get; set; } = null!;
        public string Content { get; set; } = null!;
        public bool Enabled { get; set; }
        public DateTime RegistrationStartDateTime { get; set; }
        public DateTime RegistrationEndDateTime { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string CreatedUserId { get; set; } = null!;
        public DateTime UpdatedDateTime { get; set; }
        public string UpdatedUserId { get; set; } = null!;

        public virtual ActivityCategory ActivityCategory { get; set; }
        public virtual ApplicationUser CreatedUser { get; set; }
        public virtual ApplicationUser UpdateUser { get; set; }
        public virtual ICollection<ActivityImage> ActivityImages { get; set; }
        public virtual ICollection<ActivityFile> ActivityFiles { get; set; }
        public virtual ICollection<ParticipatedActivityRecord> ParticipatedActivityRecords { get; set; }
    }
}