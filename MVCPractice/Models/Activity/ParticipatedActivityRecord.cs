using MVCPractice.Models.Account;

namespace MVCPractice.Models.Activities
{
    public class ParticipatedActivityRecord
    {
        public Guid Id { get; set; }
        public Guid ActivityId { get; set; }
        public int PersonsNumber { get; set; }
        public bool IsCancel { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public String CreatedUserId { get; set; }
        public virtual ActivityData ActivityData { get; set; }
        public virtual ApplicationUser CreatedUser { get; set; }
    }
}