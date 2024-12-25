using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MVCPractice.Models.Activities
{
    public class ActivityData
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "請輸入活動名稱")]
        public string Name { get; set; } = null!;
        public int? OrderIndex { get; set; }
        public int? CategoryId { get; set; }
        public int? PersonsNumber { get; set; }
        public string Introduce { get; set; }
        [Column(TypeName = "nvarchar(MAX)")]
        public string Content { get; set; }
        public bool Enabled { get; set; }
        public DateTime RegistrationStartDateTime { get; set; }
        public DateTime RegistrationEndDateTime { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string CreatedUserName { get; set; }
        public DateTime UpdatedDateTime { get; set; }
        public string UpdatedUserName { get; set; }
    }
}
