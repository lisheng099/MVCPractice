using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCPractice.Models.Activities
{
    public class Activitiy
    {
        public ActivityData ActivityData { get; set; }
        public List<ActivityImage> ActivityImages { get; set; }
        public List<ActivityFile> ActivityFiles { get; set; }
    }
}
