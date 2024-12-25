using MVCPractice.Models.Activities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MVCPractice.ViewModels.Activities
{
    public class ActivityInfoViewModel
    {

        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int? OrderIndex { get; set; }
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int? ParticipatedPersonsNumber { get; set; }
        public int? PersonsNumber { get; set; }
        public string Introduce { get; set; }
        [Column(TypeName = "nvarchar(MAX)")]
        public string Content { get; set; }
        public bool Enabled { get; set; }
        public DateTime? RegistrationEndDateTime { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }


        public string CoverImageUrl { get; set; }

        public List<ActivityImage> ActivityImages { get; set; }
        public List<ActivityFile> ActivityFiles { get; set; }
    }
}
