using System.ComponentModel.DataAnnotations;

namespace MVCPractice.Models.Activities
{
    public class ActivityFile
    {
        [Key]
        public int Id { get; set; }
        public int ActivityId { get; set; }
        public string Name { get; set; }
        public int? OrderIndex { get; set; }
        public string Url { get; set; }
    }
}
