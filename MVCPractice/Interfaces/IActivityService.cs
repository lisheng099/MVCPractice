using MVCPractice.Dtos.Activities;
using MVCPractice.Parameters.Activity;
using MVCPractice.ViewModels.Activities;

namespace MVCPractice.Interfaces
{
    public interface IActivityService
    {
        public Task<List<ActivityCategoryDto>> GetActivityCategories();

        public Task<List<ActivityInfoDto>> GetActivityInfos(GetActivityParameter value = null);
        public Task<bool> CheckParticipatedActivityInfoById (CheckParticipatedActivityParameter value);
        public Task<bool> ParticipatedActivityById(ParticipatedActivityDto value);
        public Task<bool> CancelParticipatedActivityById(CancelParticipatedActivityDto value);


        public Task<ActivitiyViewModel> GetActivitiesByActivityId(Guid ActivityId);
        public Task<ActivityDataDto> GetActivityDataByActivityId(Guid ActivityId);
        public Task SwitchActivityEnabledById(Guid ActivityId);

        public Task<Guid> UpdateActivityData(ActivityDataDto Activity);

        public Task UpdateActivityImage(ActivityImageDto activityImage);
        public Task DeleteActivityImageById(Guid ActivityImageId);
        public Task SwitchActivityImageIsCoverById(Guid ActivityImageId);
        public Task UpdateActivityFile(ActivityFileDto activityFile);
        public Task DeleteActivityFileById(Guid ActivityFileId);

        public Task UploadActivityImages(Guid ActivityId, List<IFormFile> images);
        public Task UploadActivityFiles(Guid ActivityId, List<IFormFile> files);
    }
}