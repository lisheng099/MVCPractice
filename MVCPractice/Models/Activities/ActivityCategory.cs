using System.ComponentModel.DataAnnotations;

namespace MVCPractice.Models.Activities
{
    public class ActivityCategory
    {
        [Key]
        public int Id { get; set; }
        public int OrderIndex { get; set; }
        public string Name { get; set; } = null!;
    }
}
