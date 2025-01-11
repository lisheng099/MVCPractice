using System.ComponentModel.DataAnnotations;

namespace MVCPractice.Models.Activities
{
    public class ActivityImage
    {
        [Key]
        public Guid Id { get; set; }

        public Guid ActivityId { get; set; }
        public string Name { get; set; }
        public bool IsCover { get; set; }
        public int OrderIndex { get; set; }
        public string Url { get; set; }

        public virtual ActivityData ActivityData { get; set; }
    }
}