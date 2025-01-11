using MVCPractice.Dtos.Activities;

namespace MVCPractice.ViewModels.Activities
{
    public class ActivitiyViewModel
    {
        public ActivityDataDto ActivityDataDto { get; set; }
        public List<ActivityImageDto> ActivityImages { get; set; }
        public List<ActivityFileDto> ActivityFiles { get; set; }
    }
}