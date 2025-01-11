using MVCPractice.Dtos.Activities;
using MVCPractice.Models.Activities;
using System.Linq;

namespace MVCPractice.Mappers
{
    public static class ActivityMapper
    {
        public static ActivityImageDto ActivityImageDataToDto(this ActivityImage x)
        {
            return new ActivityImageDto
            {
                Id = x.Id,
                ActivityId = x.ActivityId,
                Name = x.Name,
                IsCover = x.IsCover,
                OrderIndex = x.OrderIndex,
                Url = x.Url
            };
        }

        public static ActivityFileDto ActivityFileDataToDto(this ActivityFile x)
        {
            return new ActivityFileDto
            {
                Id = x.Id,
                ActivityId = x.ActivityId,
                Name = x.Name,
                OrderIndex = x.OrderIndex,
                Url = x.Url
            };
        }

        public static ActivityInfoDto ActivityDataToActivityInfoDto(this ActivityData x)
        {
            return 
                new ActivityInfoDto()
                {
                    ActivityId = x.Id,
                    Name = x.Name,
                    OrderIndex = x.OrderIndex,
                    CategoryId = x.CategoryId,
                    CategoryName = x.ActivityCategory.Name ?? "",
                    ParticipatedPersonsNumber = x.ParticipatedActivityRecords.Where(x=>!x.IsCancel).Select(x => x.PersonsNumber).Count(),
                    PersonsNumber = x.PersonsNumber,
                    Introduce = x.Introduce,
                    Content = x.Content,
                    Enabled = x.Enabled,
                    RegistrationEndDateTime = x.RegistrationEndDateTime,
                    StartDateTime = x.StartDateTime,
                    EndDateTime = x.EndDateTime,
                    CoverImageUrl = x.ActivityImages != null && x.ActivityImages.Any()
                            ? x.ActivityImages.Where(img => img.IsCover).FirstOrDefault()?.Url ?? "/image/Activities/Default.jpg"
                            : "/image/Activities/Default.jpg",
                    ActivityImages = x.ActivityImages.Select(x => x.ActivityImageDataToDto()).ToList(),
                    ActivityFiles = x.ActivityFiles.Select(x => x.ActivityFileDataToDto()).ToList()
                };
        }

    }
}