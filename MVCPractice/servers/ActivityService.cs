using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MVCPractice.Dtos.Activities;
using MVCPractice.Interfaces;
using MVCPractice.Models;
using MVCPractice.Models.Activities;
using MVCPractice.ViewModels.Activities;

namespace MVCPractice.servers
{
    public class ActivityService(MVCPracticeDBContext context,
        IWebHostEnvironment webHostEnvironment) : IActivityService
    {
        private readonly MVCPracticeDBContext _context = context;
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

        public async Task<List<ActivityCategoryDto>> GetActivityCategories()
        {
            List<ActivityCategoryDto> ActivityCategories = await (from a in _context.ActivityCategorys
                                                            select new ActivityCategoryDto
                                                            {
                                                                Id = a.Id,
                                                                OrderIndex = a.OrderIndex,
                                                                Name = a.Name,
                                                            }).ToListAsync();

            return ActivityCategories;
        }
        public List<ActivityInfoDto> GetActivityInfos()
        {
            List<ActivityInfoDto> activityInfos = (from ActivityData in _context.ActivityDatas
                                                   join ActivityImage in _context.ActivityImages.Where(x => x.IsCover)
                                                   on ActivityData.Id equals ActivityImage.ActivityId into activityImageGroup
                                                   from activityImage in activityImageGroup.DefaultIfEmpty()
                                                   select new ActivityInfoDto()
                                                   {
                                                       Id = ActivityData.Id,
                                                       Name = ActivityData.Name,
                                                       OrderIndex = ActivityData.OrderIndex ?? 99,
                                                       CategoryId = ActivityData.CategoryId ?? 0,
                                                       CategoryName = _context.ActivityCategorys.Where(x => x.Id == ActivityData.CategoryId).First().Name,
                                                       ParticipatedPersonsNumber = _context.ParticipatedActivityRecords.Where(x => x.ActivityId == ActivityData.Id && (!x.IsCancel ?? true)).Count(),
                                                       PersonsNumber = ActivityData.PersonsNumber ?? 0,
                                                       Introduce = ActivityData.Introduce,
                                                       Content = ActivityData.Content,
                                                       Enabled = ActivityData.Enabled,
                                                       RegistrationEndDateTime = ActivityData.RegistrationEndDateTime,
                                                       StartDateTime = ActivityData.StartDateTime,
                                                       EndDateTime = ActivityData.EndDateTime,
                                                       CoverImageUrl = activityImage.Url ?? "/image/Activities/Default.jpg",
                                                       ActivityImages = _context.ActivityImages.Where(x => x.ActivityId == ActivityData.Id)
                                                       .Select(x => new ActivityImageDto {
                                                           Id = x.Id,
                                                           ActivityId = x.ActivityId,
                                                           Name = x.Name,
                                                           IsCover = x.IsCover,
                                                           OrderIndex = x.OrderIndex ?? 99,
                                                           Url = x.Url
                                                       }).ToList(),
                                                       ActivityFiles = _context.ActivityFiles.Where(x => x.ActivityId == ActivityData.Id)
                                                       .Select(x=>new ActivityFileDto 
                                                       {
                                                           Id = x.Id,
                                                           ActivityId = x.ActivityId,
                                                           Name=x.Name,
                                                           OrderIndex = x.OrderIndex ?? 99,
                                                           Url = x.Url
                                                       }).ToList(),
                                                   }
                                                ).ToList();
            return activityInfos;
        }

        public List<ActivityInfoDto> GetActivityInfos(DateTime dateTime)
        {
            List<ActivityInfoDto> activityInfos = (from ActivityData in _context.ActivityDatas.Where(x => x.RegistrationStartDateTime < dateTime && x.RegistrationEndDateTime > dateTime && x.Enabled)
                                                join ActivityImage in _context.ActivityImages.Where(x => x.IsCover)
                                                on ActivityData.Id equals ActivityImage.ActivityId into activityImageGroup
                                                from activityImage in activityImageGroup.DefaultIfEmpty()
                                                select new ActivityInfoDto()
                                                {
                                                    Id = ActivityData.Id,
                                                    Name = ActivityData.Name,
                                                    OrderIndex = ActivityData.OrderIndex ?? 0,
                                                    CategoryId = ActivityData.CategoryId ?? 0,
                                                    CategoryName = _context.ActivityCategorys.Where(x => x.Id == ActivityData.CategoryId).First().Name,
                                                    ParticipatedPersonsNumber = _context.ParticipatedActivityRecords.Where(x => x.ActivityId == ActivityData.Id && (!x.IsCancel ?? true)).Count(),
                                                    PersonsNumber = ActivityData.PersonsNumber ?? 0,
                                                    Introduce = ActivityData.Introduce,
                                                    Content = ActivityData.Content,
                                                    RegistrationEndDateTime = ActivityData.RegistrationEndDateTime,
                                                    StartDateTime = ActivityData.StartDateTime,
                                                    EndDateTime = ActivityData.EndDateTime,
                                                    CoverImageUrl = activityImage.Url ?? "/image/Activities/Default.jpg",
                                                    ActivityImages = _context.ActivityImages.Where(x => x.ActivityId == ActivityData.Id)
                                                       .Select(x => new ActivityImageDto
                                                       {
                                                           Id = x.Id,
                                                           ActivityId = x.ActivityId,
                                                           Name = x.Name,
                                                           IsCover = x.IsCover,
                                                           OrderIndex = x.OrderIndex ?? 99,
                                                           Url = x.Url
                                                       }).ToList(),
                                                    ActivityFiles = _context.ActivityFiles.Where(x => x.ActivityId == ActivityData.Id)
                                                       .Select(x => new ActivityFileDto
                                                       {
                                                           Id = x.Id,
                                                           ActivityId = x.ActivityId,
                                                           Name = x.Name,
                                                           OrderIndex = x.OrderIndex ?? 99,
                                                           Url = x.Url
                                                       }).ToList(),
                                                }
                                                ).ToList();
            return activityInfos;
        }

        public List<ActivityInfoDto> GetParticipatedActivityInfos(string UserName)
        {
            List<ActivityInfoDto> activityInfos = (from ParticipatedActivityRecord in _context.ParticipatedActivityRecords.Where(x => x.UserName == UserName)
                                                   join ActivityData in _context.ActivityDatas
                                                   on ParticipatedActivityRecord.ActivityId equals ActivityData.Id into activityDataGroup
                                                   from ActivityData in activityDataGroup.DefaultIfEmpty()
                                                   join ActivityImage in _context.ActivityImages.Where(x => x.IsCover)
                                                   on ActivityData.Id equals ActivityImage.ActivityId into activityImageGroup
                                                   from activityImage in activityImageGroup.DefaultIfEmpty()
                                                   select new ActivityInfoDto()
                                                   {
                                                       Id = ActivityData.Id,
                                                       Name = ActivityData.Name,
                                                       OrderIndex = ActivityData.OrderIndex ?? 99,
                                                       CategoryId = ActivityData.CategoryId ?? 0,
                                                       CategoryName = _context.ActivityCategorys.Where(x => x.Id == ActivityData.CategoryId).First().Name,
                                                       ParticipatedPersonsNumber = _context.ParticipatedActivityRecords.Where(x => x.ActivityId == ActivityData.Id && (!x.IsCancel ?? true)).Count(),
                                                       PersonsNumber = ActivityData.PersonsNumber ?? 0,
                                                       Introduce = ActivityData.Introduce,
                                                       Content = ActivityData.Content,
                                                       StartDateTime = ActivityData.StartDateTime,
                                                       EndDateTime = ActivityData.EndDateTime,
                                                       CoverImageUrl = activityImage.Url ?? "/image/Activities/Default.jpg",
                                                       ActivityImages = _context.ActivityImages.Where(x => x.ActivityId == ActivityData.Id)
                                                       .Select(x => new ActivityImageDto
                                                       {
                                                           Id = x.Id,
                                                           ActivityId = x.ActivityId,
                                                           Name = x.Name,
                                                           IsCover = x.IsCover,
                                                           OrderIndex = x.OrderIndex ?? 99,
                                                           Url = x.Url
                                                       }).ToList(),
                                                       ActivityFiles = _context.ActivityFiles.Where(x => x.ActivityId == ActivityData.Id)
                                                       .Select(x => new ActivityFileDto
                                                       {
                                                           Id = x.Id,
                                                           ActivityId = x.ActivityId,
                                                           Name = x.Name,
                                                           OrderIndex = x.OrderIndex ?? 99,
                                                           Url = x.Url
                                                       }).ToList(),
                                                   }
                                                ).ToList();
            return activityInfos;
        }

        public ActivityInfoDto GetActivityInfoById(int ActivityId)
        {
            List<ActivityInfoDto> activityInfos = (from ActivityData in _context.ActivityDatas.Where(x => x.Id == ActivityId)
                                                join ActivityImage in _context.ActivityImages.Where(x => x.IsCover)
                                                on ActivityData.Id equals ActivityImage.ActivityId into activityImageGroup
                                                from activityImage in activityImageGroup.DefaultIfEmpty()
                                                select new ActivityInfoDto()
                                                {
                                                    Id = ActivityData.Id,
                                                    Name = ActivityData.Name,
                                                    OrderIndex = ActivityData.OrderIndex ?? 99,
                                                    CategoryId = ActivityData.CategoryId ?? 0,
                                                    CategoryName = _context.ActivityCategorys.Where(x => x.Id == ActivityData.CategoryId).First().Name,
                                                    ParticipatedPersonsNumber = 0,
                                                    PersonsNumber = ActivityData.PersonsNumber ?? 0,
                                                    Introduce = ActivityData.Introduce,
                                                    Content = ActivityData.Content,
                                                    StartDateTime = ActivityData.StartDateTime,
                                                    EndDateTime = ActivityData.EndDateTime,
                                                    CoverImageUrl = activityImage.Url ?? "/image/Activities/Default.jpg",
                                                    ActivityImages = _context.ActivityImages.Where(x => x.ActivityId == ActivityData.Id)
                                                       .Select(x => new ActivityImageDto
                                                       {
                                                           Id = x.Id,
                                                           ActivityId = x.ActivityId,
                                                           Name = x.Name,
                                                           IsCover = x.IsCover,
                                                           OrderIndex = x.OrderIndex ?? 99,
                                                           Url = x.Url
                                                       }).ToList(),
                                                    ActivityFiles = _context.ActivityFiles.Where(x => x.ActivityId == ActivityData.Id)
                                                       .Select(x => new ActivityFileDto
                                                       {
                                                           Id = x.Id,
                                                           ActivityId = x.ActivityId,
                                                           Name = x.Name,
                                                           OrderIndex = x.OrderIndex ?? 99,
                                                           Url = x.Url
                                                       }).ToList(),
                                                }
                                                ).ToList();

            return activityInfos.First();
        }
        public bool CheckParticipatedActivityInfoById(int ActivityId, string UserName)
        {
            List<ParticipatedActivityRecord> participatedActivityRecords = (from a in _context.ParticipatedActivityRecords
                                                                            where a.ActivityId == ActivityId && a.UserName == UserName
                                                                            select a).ToList();
            if(participatedActivityRecords.Count == 0)
            {
                return false;
            }
            return true;
        }

        public bool ParticipatedActivityById(int ActivityId, string UserName, int PersonsNumber,string CreatedUserName)
        {
            ParticipatedActivityRecord participatedActivityRecord =
                new ParticipatedActivityRecord()
                {
                    ActivityId = ActivityId,
                    UserName = UserName,
                    PersonsNumber = PersonsNumber,
                    IsCancel = false,
                    CreatedDateTime = DateTime.Now,
                    CreatedUserName = CreatedUserName,
                };
            List<ActivityData> activityDatas = _context.ActivityDatas.Where(x => x.Id == participatedActivityRecord.ActivityId).ToList();
            if (activityDatas.Count > 0)
            {
                ActivityData activityData = activityDatas.First();
                List<ParticipatedActivityRecord> participatedActivityRecords = _context.ParticipatedActivityRecords.Where(x => x.ActivityId == participatedActivityRecord.ActivityId).ToList();
                int participatedActivityRecordsNumber = participatedActivityRecords.Where(x => !(x.IsCancel ?? false) && x.UserName != participatedActivityRecord.UserName).Sum(x => x.PersonsNumber ?? 0);

                if ((participatedActivityRecordsNumber + participatedActivityRecord.PersonsNumber) <= activityData.PersonsNumber)
                {
                    if (participatedActivityRecords.Where(x => x.UserName == participatedActivityRecord.UserName).Count() > 0)
                    {
                        ParticipatedActivityRecord oldParticipatedActivityRecord =
                            _context.ParticipatedActivityRecords.Where(x =>
                            x.ActivityId == participatedActivityRecord.ActivityId &&
                            x.UserName == participatedActivityRecord.UserName).First();
                        oldParticipatedActivityRecord.PersonsNumber = participatedActivityRecord.PersonsNumber;
                        oldParticipatedActivityRecord.IsCancel = participatedActivityRecord.IsCancel;
                        oldParticipatedActivityRecord.CreatedDateTime = participatedActivityRecord.CreatedDateTime;
                        oldParticipatedActivityRecord.CreatedUserName = participatedActivityRecord.CreatedUserName;
                    }
                    else
                    {
                        _context.ParticipatedActivityRecords.Add(participatedActivityRecord);
                    }

                    if (_context.SaveChanges() > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return false;
        }
        public bool CancelParticipatedActivityById(int ActivityId, string UserName, int PersonsNumber, string CreatedUserName)
        {
            List<ParticipatedActivityRecord> participatedActivityRecords = (from a in _context.ParticipatedActivityRecords
                                                                    where a.ActivityId == ActivityId && a.UserName == UserName
                                                                    select a).ToList();
            if (participatedActivityRecords.Count != 0)
            {
                ParticipatedActivityRecord participatedActivityRecord = participatedActivityRecords.First();
                _context.ParticipatedActivityRecords.Remove(participatedActivityRecord);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
        public ActivitiyViewModel GetActivitiesByActivityId(int ActivityId)
        {
            ActivitiyViewModel Activities = new();
            Activities.ActivityDataDto = GetActivityDataByActivityId(ActivityId);
            Activities.ActivityImages = GetActivityImagesByActivityId(ActivityId);
            Activities.ActivityFiles = GetActivityFilesByActivityId(ActivityId);
            return Activities;
        }

        public ActivityDataDto GetActivityDataByActivityId(int ActivityId)
        {
            List<ActivityData> Activities = _context.ActivityDatas.Where(x => x.Id == ActivityId).ToList();
            if (Activities.Count != 0)
            {
                ActivityData Activity = Activities.First();
                ActivityDataDto activityDataDto = new ActivityDataDto()
                {
                    Id = Activity.Id,
                    Name =Activity.Name,
                    OrderIndex = Activity.OrderIndex,
                    CategoryId = Activity.CategoryId,
                    PersonsNumber = Activity.PersonsNumber,
                    Introduce = Activity.Introduce,
                    Content = Activity.Content,
                    Enabled = Activity.Enabled,
                    RegistrationStartDateTime = Activity.RegistrationStartDateTime,
                    RegistrationEndDateTime = Activity.RegistrationEndDateTime,
                    StartDateTime = Activity.StartDateTime,
                    EndDateTime = Activity.EndDateTime,
                    CreatedDateTime = Activity.CreatedDateTime,
                    CreatedUserName = Activity.CreatedUserName,
                    UpdatedDateTime = Activity.UpdatedDateTime,
                    UpdatedUserName = Activity.UpdatedUserName,
                };
                return activityDataDto;
            }
            else
            {
                return null;
            }
        }
        public List<ActivityImageDto> GetActivityImagesByActivityId(int ActivityId)
        {
            List<ActivityImageDto> Images = (from a in _context.ActivityImages 
                                            where a.ActivityId == ActivityId
                                            select new ActivityImageDto
                                            {
                                                Id = a.Id,
                                                ActivityId = ActivityId,
                                                Name = a.Name,
                                                IsCover = a.IsCover,
                                                OrderIndex = a.OrderIndex ?? 99,
                                                Url = a.Url,
                                            }).ToList();
            return Images;
        }
        public List<ActivityFileDto> GetActivityFilesByActivityId(int ActivityId)
        {
            List<ActivityFileDto> Files = (from a in _context.ActivityFiles 
                                           where a.ActivityId == ActivityId
                                           select new ActivityFileDto
                                           {
                                               Id = a.Id,
                                               ActivityId = a.ActivityId,
                                               Name = a.Name,
                                               OrderIndex = a.OrderIndex ?? 99,
                                               Url = a.Url,
                                           }).ToList();
            return Files;
        }

        public void SwitchActivityEnabledById(int ActivityId)
        {
            List<ActivityData> Activitys = _context.ActivityDatas.Where(x => x.Id == ActivityId).ToList();
            if (Activitys.Count != 0)
            {
                ActivityData Activity = Activitys.First();
                Activity.Enabled = !Activity.Enabled;
                Activity.UpdatedDateTime = DateTime.Now;
                _context.SaveChanges();
            }
        }

        public int UpdateActivityData(ActivityDataDto Activity)
        {
            List<ActivityData> Activitys = _context.ActivityDatas.Where(x => x.Id == Activity.Id).ToList();
            if (Activitys.Count != 0)
            {
                ActivityData oldActivity = Activitys.First();
                oldActivity.Name = Activity.Name;
                oldActivity.OrderIndex = Activity.OrderIndex;
                oldActivity.CategoryId = Activity.CategoryId;
                oldActivity.PersonsNumber = Activity.PersonsNumber;
                oldActivity.Introduce = Activity.Introduce;
                oldActivity.Content = Activity.Content;
                oldActivity.Enabled = Activity.Enabled;
                oldActivity.RegistrationStartDateTime = Activity.RegistrationStartDateTime;
                oldActivity.RegistrationEndDateTime = Activity.RegistrationEndDateTime;
                oldActivity.StartDateTime = Activity.StartDateTime;
                oldActivity.EndDateTime = Activity.EndDateTime;
                oldActivity.UpdatedDateTime = DateTime.Now;
                oldActivity.UpdatedUserName = Activity.UpdatedUserName;
                _context.SaveChanges();
                return Activity.Id;
            }
            else
            {
                ActivityData newActivity = new ActivityData();
                newActivity.Name = Activity.Name;
                newActivity.OrderIndex = Activity.OrderIndex;
                newActivity.CategoryId = Activity.CategoryId;
                newActivity.PersonsNumber = Activity.PersonsNumber;
                newActivity.Introduce = Activity.Introduce;
                newActivity.Content = Activity.Content;
                newActivity.Enabled = Activity.Enabled;
                newActivity.RegistrationStartDateTime = Activity.RegistrationStartDateTime;
                newActivity.RegistrationEndDateTime = Activity.RegistrationEndDateTime;
                newActivity.StartDateTime = Activity.StartDateTime;
                newActivity.EndDateTime = Activity.EndDateTime;
                newActivity.CreatedDateTime = DateTime.Now;
                newActivity.CreatedUserName = Activity.CreatedUserName;

                _context.ActivityDatas.Add(newActivity);
                _context.SaveChanges();
                return newActivity.Id;
            }

        }

        public void AddRegisterImage(ActivityImage activityImage)
        {
            _context.ActivityImages.Add(activityImage);
            _context.SaveChanges();
        }
        public void AddRegisterFile(ActivityFile activityFile)
        {
            _context.ActivityFiles.Add(activityFile);
            _context.SaveChanges();
        }
        public void UpdateActivityImage(ActivityImageDto activityImage)
        {
            List<ActivityImage> ActivityImages = _context.ActivityImages.Where(x => x.Id == activityImage.Id).ToList();
            if (ActivityImages.Count != 0)
            {
                ActivityImage oldActivityImage = ActivityImages.First();
                oldActivityImage.Name = activityImage.Name;
                oldActivityImage.OrderIndex = activityImage.OrderIndex;
                _context.SaveChanges();
            }
        }
        public void DeleteActivityImageById(int ActivityImageId)
        {
            List<ActivityImage> ActivityImages = _context.ActivityImages.Where(x => x.Id == ActivityImageId).ToList();
            if (ActivityImages.Count != 0)
            {
                ActivityImage ActivityImage = ActivityImages.First();
                _context.ActivityImages.Remove(ActivityImage);
                _context.SaveChanges();
            }
        }
        public void SwitchActivityImageIsCoverById(int ActivityImageId)
        {
            List<ActivityImage> ActivityImages = _context.ActivityImages.Where(x => x.Id == ActivityImageId).ToList();
            if (ActivityImages.Count != 0)
            {
                ActivityImage ActivityImage = ActivityImages.First();
                ActivityImages = _context.ActivityImages.Where(x => x.ActivityId == ActivityImage.ActivityId).ToList();
                foreach (var item in ActivityImages)
                {
                    item.IsCover = false;
                }
                ActivityImage.IsCover = true;
                _context.SaveChanges();
            }
        }
        public void UpdateActivityFile(ActivityFileDto activityFile)
        {
            List<ActivityFile> ActivityFiles = _context.ActivityFiles.Where(x => x.Id == activityFile.Id).ToList();
            if (ActivityFiles.Count != 0)
            {
                ActivityFile oldActivityFile = ActivityFiles.First();
                oldActivityFile.Name = activityFile.Name;
                oldActivityFile.OrderIndex = activityFile.OrderIndex;
                _context.SaveChanges();
            }
        }
        public void DeleteActivityFileById(int ActivityFileId)
        {
            List<ActivityFile> ActivityFiles = _context.ActivityFiles.Where(x => x.Id == ActivityFileId).ToList();
            if (ActivityFiles.Count != 0)
            {
                ActivityFile ActivityFile = ActivityFiles.First();
                _context.ActivityFiles.Remove(ActivityFile);
                _context.SaveChanges();
            }
        }

        public async void UploadActivityImages(int ActivityId, List<IFormFile> images)
        {
            var uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "image/Activities");

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            foreach (var image in images)
            {
                if (image.Length > 0)
                {
                    var FileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                    var filePath = Path.Combine(uploadPath, FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                    AddRegisterImage(
                        new ActivityImage()
                        {
                            ActivityId = ActivityId,
                            Name = image.FileName,
                            IsCover = false,
                            OrderIndex = 0,
                            Url = "/image/Activities/" + FileName,
                        });
                }
            }
        }

        public async void UploadActivityFiles(int ActivityId, List<IFormFile> files)
        {
            var uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "file/Activities");

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            foreach (var file in files)
            {

                var FileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(uploadPath, FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                AddRegisterFile(
                    new ActivityFile()
                    {
                        ActivityId = ActivityId,
                        Name = file.FileName,
                        OrderIndex = 0,
                        Url = "/file/Activities/" + FileName,
                    });

            }
        }

        
    }
}
