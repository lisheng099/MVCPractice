using Microsoft.EntityFrameworkCore;
using MVCPractice.Dtos.Activities;
using MVCPractice.Interfaces;
using MVCPractice.Mappers;
using MVCPractice.Models;
using MVCPractice.Models.Activities;
using MVCPractice.Parameters.Activity;
using MVCPractice.ViewModels.Activities;
using System.Diagnostics;

namespace MVCPractice.services
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

        public async Task<List<ActivityInfoDto>> GetActivityInfos(GetActivityParameter value = null)
        {
            var result =
                _context.ActivityDatas
                .Include(x => x.ActivityCategory)
                .Include(x => x.ActivityImages)
                .Include(x => x.ActivityFiles)
                .Include(x => x.ParticipatedActivityRecords.Where(x => !x.IsCancel))
                .Select(x => x);

            if (value == null) 
            {
                return result.Select(x => x.ActivityDataToActivityInfoDto()).ToList();
            }

            if (value.ActivityId != null)
            {
                result = result.Where(x => x.Id == value.ActivityId);
            }

            if (value.CategoryId != null)
            {
                result = result.Where(x => x.CategoryId == value.CategoryId);
            }

            if (value.StartDateTime != null && value.StartDateTime != null)
            {
                result = result.Where(x =>
                    (x.RegistrationStartDateTime >= value.StartDateTime && x.RegistrationStartDateTime <= value.EndDateTime) ||
                    (x.RegistrationEndDateTime >= value.StartDateTime && x.RegistrationEndDateTime <= value.EndDateTime) ||
                    (x.RegistrationStartDateTime <= value.StartDateTime && x.RegistrationEndDateTime >= value.EndDateTime)
                );
            }

            if (value.UserId != null)
            {
                result = result.Where(x => x.ParticipatedActivityRecords.Any(x => x.CreatedUserId == value.UserId));
            }

            if (value.CheckIsParticipated)
            {
                result = result.Where(x => x.ParticipatedActivityRecords.Any(x => x.CreatedUserId == value.UserId && !x.IsCancel));
            }

            return await result.Select(x=>x.ActivityDataToActivityInfoDto()).ToListAsync();
        }

        public async Task<bool> CheckParticipatedActivityInfoById(CheckParticipatedActivityParameter value)
        {
            var result =
                _context.ActivityDatas
                .Include(x => x.ParticipatedActivityRecords)
                .Where(x=>x.Id == value.ActivityId)
                .Where(x => x.ParticipatedActivityRecords.Any(x =>x.CreatedUserId == value.UserId && !x.IsCancel))
                .Select(x => x);

            if ((await result.ToListAsync()).Count == 0)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> ParticipatedActivityById(ParticipatedActivityDto value)
        {
            int PersonsNumber = _context.ActivityDatas.Where(x=>x.Id == value.ActivityId).Count();
            int CurParticipatedPersonsNumber = _context.ParticipatedActivityRecords.Where(x => x.ActivityId == value.ActivityId && !x.IsCancel).Select(x => x.PersonsNumber).Count();
            var exist = _context.ParticipatedActivityRecords.Where(x => x.ActivityId == value.ActivityId && x.CreatedUserId == value.UserId).FirstOrDefault();
            if (exist == null)
            {
                if(PersonsNumber > CurParticipatedPersonsNumber + value.PersonsNumber)
                {
                    return false;
                }
                else
                {
                    _context.ParticipatedActivityRecords.Add(new ParticipatedActivityRecord()
                    {
                        ActivityId = value.ActivityId,
                        PersonsNumber = PersonsNumber,
                        IsCancel = false,
                        CreatedDateTime = DateTime.Now,
                        CreatedUserId = value.UserId
                    });
                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                exist.IsCancel = false;
                exist.PersonsNumber = PersonsNumber;
                await _context.SaveChangesAsync();
            }
            return true;
        }

        public async Task<bool> CancelParticipatedActivityById(CancelParticipatedActivityDto value)
        {
            List<ParticipatedActivityRecord> participatedActivityRecords = (from a in _context.ParticipatedActivityRecords
                                                                            where a.ActivityId == value.ActivityId && a.CreatedUserId == value.UserId
                                                                            select a).ToList();
            if (participatedActivityRecords.Count != 0)
            {
                ParticipatedActivityRecord participatedActivityRecord = participatedActivityRecords.First();
                participatedActivityRecord.IsCancel = true;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }


        
        public async Task<ActivitiyViewModel> GetActivitiesByActivityId(Guid ActivityId)
        {
            ActivitiyViewModel Activities = new();
            Activities.ActivityDataDto = await GetActivityDataByActivityId(ActivityId);
            Activities.ActivityImages = await GetActivityImagesByActivityId(ActivityId);
            Activities.ActivityFiles = await GetActivityFilesByActivityId(ActivityId);
            return Activities;
        }

        public async Task<ActivityDataDto> GetActivityDataByActivityId(Guid ActivityId)
        {
            List<ActivityData> Activities = await _context.ActivityDatas.Where(x => x.Id == ActivityId).ToListAsync();
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
                    CreatedUserId = Activity.CreatedUserId,
                    UpdatedDateTime = Activity.UpdatedDateTime,
                    UpdatedUserId = Activity.UpdatedUserId,
                };
                return activityDataDto;
            }
            else
            {
                return null;
            }
        }
        public async Task<List<ActivityImageDto>> GetActivityImagesByActivityId(Guid ActivityId)
        {
            List<ActivityImageDto> Images = await (from a in _context.ActivityImages
                                            where a.ActivityId == ActivityId
                                            select new ActivityImageDto
                                            {
                                                Id = a.Id,
                                                ActivityId = ActivityId,
                                                Name = a.Name,
                                                IsCover = a.IsCover,
                                                OrderIndex = a.OrderIndex,
                                                Url = a.Url,
                                            }).ToListAsync();
            return Images;
        }
        public async Task<List<ActivityFileDto>> GetActivityFilesByActivityId(Guid ActivityId)
        {
            List<ActivityFileDto> Files = await (from a in _context.ActivityFiles
                                           where a.ActivityId == ActivityId
                                           select new ActivityFileDto
                                           {
                                               Id = a.Id,
                                               ActivityId = a.ActivityId,
                                               Name = a.Name,
                                               OrderIndex = a.OrderIndex,
                                               Url = a.Url,
                                           }).ToListAsync();
            return Files;
        }

        public async Task SwitchActivityEnabledById(Guid ActivityId)
        {
            List<ActivityData> Activitys = _context.ActivityDatas.Where(x => x.Id == ActivityId).ToList();
            if (Activitys.Count != 0)
            {
                ActivityData Activity = Activitys.First();
                Activity.Enabled = !Activity.Enabled;
                Activity.UpdatedDateTime = DateTime.Now;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Guid> UpdateActivityData(ActivityDataDto Activity)
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
                oldActivity.UpdatedUserId = Activity.UpdatedUserId;
                await _context.SaveChangesAsync();
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
                newActivity.CreatedUserId = Activity.CreatedUserId;
                newActivity.UpdatedDateTime = DateTime.Now;
                newActivity.UpdatedUserId = Activity.CreatedUserId;

                _context.ActivityDatas.Add(newActivity);
                await _context.SaveChangesAsync();
                return newActivity.Id;
            }
        }

        public async Task AddRegisterImage(ActivityImage activityImage)
        {
            _context.ActivityImages.Add(activityImage);
            await _context.SaveChangesAsync();
        }
        public async Task AddRegisterFile(ActivityFile activityFile)
        {
            _context.ActivityFiles.Add(activityFile);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateActivityImage(ActivityImageDto activityImage)
        {
            List<ActivityImage> ActivityImages = _context.ActivityImages.Where(x => x.Id == activityImage.Id).ToList();
            if (ActivityImages.Count != 0)
            {
                ActivityImage oldActivityImage = ActivityImages.First();
                oldActivityImage.Name = activityImage.Name;
                oldActivityImage.OrderIndex = activityImage.OrderIndex;
                await _context.SaveChangesAsync();
            }
        }
        public async Task DeleteActivityImageById(Guid ActivityImageId)
        {
            List<ActivityImage> ActivityImages = _context.ActivityImages.Where(x => x.Id == ActivityImageId).ToList();
            if (ActivityImages.Count != 0)
            {
                ActivityImage ActivityImage = ActivityImages.First();
                _context.ActivityImages.Remove(ActivityImage);
                await _context.SaveChangesAsync();
            }
        }
        public async Task SwitchActivityImageIsCoverById(Guid ActivityImageId)
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
                await _context.SaveChangesAsync();
            }
        }
        public async Task UpdateActivityFile(ActivityFileDto activityFile)
        {
            List<ActivityFile> ActivityFiles = _context.ActivityFiles.Where(x => x.Id == activityFile.Id).ToList();
            if (ActivityFiles.Count != 0)
            {
                ActivityFile oldActivityFile = ActivityFiles.First();
                oldActivityFile.Name = activityFile.Name;
                oldActivityFile.OrderIndex = activityFile.OrderIndex;
                await _context.SaveChangesAsync();
            }
        }
        public async Task DeleteActivityFileById(Guid ActivityFileId)
        {
            List<ActivityFile> ActivityFiles = _context.ActivityFiles.Where(x => x.Id == ActivityFileId).ToList();
            if (ActivityFiles.Count != 0)
            {
                ActivityFile ActivityFile = ActivityFiles.First();
                _context.ActivityFiles.Remove(ActivityFile);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UploadActivityImages(Guid ActivityId, List<IFormFile> images)
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

                    await AddRegisterImage(
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

        public async Task UploadActivityFiles(Guid ActivityId, List<IFormFile> files)
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

                await AddRegisterFile(
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