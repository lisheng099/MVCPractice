using MVCPractice.Dtos.Activities;
using MVCPractice.Models.Activities;
using MVCPractice.ViewModels.Activities;

namespace MVCPractice.Interfaces
{
    public interface IActivityService
    {
        Task<List<ActivityCategoryDto>> GetActivityCategories();
        List<ActivityInfoDto> GetActivityInfos();
        List<ActivityInfoDto> GetActivityInfos(DateTime dateTime);
        List<ActivityInfoDto> GetParticipatedActivityInfos(string UserName);
        ActivityInfoDto GetActivityInfoById(int ActivityId);
        bool CheckParticipatedActivityInfoById (int ActivityId, string UserName);
        bool ParticipatedActivityById(int ActivityId, string UserName, int PersonsNumber, string CreatedUserName);
        bool CancelParticipatedActivityById(int ActivityId, string UserName, int PersonsNumber, string CreatedUserName);
        ActivitiyViewModel GetActivitiesByActivityId(int ActivityId);
        ActivityDataDto GetActivityDataByActivityId(int ActivityId);
        void SwitchActivityEnabledById(int ActivityId);

        int UpdateActivityData(ActivityDataDto Activity);

        void UpdateActivityImage(ActivityImageDto activityImage);
        void DeleteActivityImageById(int ActivityImageId);
        void SwitchActivityImageIsCoverById(int ActivityImageId);
        void UpdateActivityFile(ActivityFileDto activityFile);
        void DeleteActivityFileById(int ActivityFileId);

        void UploadActivityImages(int ActivityId, List<IFormFile> images);
        void UploadActivityFiles(int ActivityId, List<IFormFile> files);
    }
}
