using System.ComponentModel.DataAnnotations;

namespace MVCPractice.Models.Activities
{
    public class ParticipatedActivityRecord
    {
        [Key]
        public int Id { get; set; }
        public int ActivityId { get; set; }
        public string UserName { get; set; }
        public int? PersonsNumber { get; set; }
        public bool? IsCancel { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string CreatedUserName { get; set; }
    }
}
