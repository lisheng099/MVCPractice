using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCPractice.Dtos.Account;
using MVCPractice.Dtos.Activities;
using MVCPractice.Dtos.Sysadm;
using MVCPractice.Interfaces;
using MVCPractice.Models.Account;
using MVCPractice.ViewModels.Account;
using MVCPractice.ViewModels.Activities;
using MVCPractice.ViewModels.Sysadm;
using System.Data;
using System.Security.Claims;

namespace MVCPractice.Controllers
{
    [Route("[controller]")]
    [Authorize(Roles = "Admin")]
    public class SysadmController(
        IAccountService accountViewModelService,
        IActivityService activityService) : Controller
    {
        private readonly IAccountService _accountService = accountViewModelService;
        private readonly IActivityService _activityService = activityService;

        
        [HttpGet("Members")]
        public async Task<IActionResult> Members(string searchTerm)
        {
            MembersViewModel model = await _accountService.GetMembersViewModel(searchTerm);
            return View(model);
        }

        [HttpGet("EditProfile/{id}")]
        public async Task<IActionResult> EditProfile(String Id)
        {
            EditProfileViewModel editProfileViewModel = await _accountService.GetEditProfileViewModel(Id);
            if(editProfileViewModel == null) { return NotFound("找不到該會員或沒有權限修改。"); }
            return View(editProfileViewModel);
        }

        [HttpPost("EditProfile/{Id}")]
        public async Task<IActionResult> EditProfile(String Id, EditProfileViewModel model)
        {
            model.Id = Id;
            if (ModelState.IsValid)
            {
                var result = await _accountService.EditProfile(model);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }

        [HttpPost("ResetPassword/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(String id)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountService.ResetPassword(id);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }
            }
            return RedirectToAction("Members");
        }

        [HttpGet("MemberRoles")]
        public async Task<IActionResult> MemberRoles(string Role)
        {
            MemberRolesViewModel model = await _accountService.GetMemberRolesViewModel(Role);
            return View(model);
        }

        [HttpPost("AddRoleByUserName")]
        public async Task<IActionResult> AddRoleByUserName(ChangeRoleDto changeRoleDto)
        {
            changeRoleDto.Add = true;
            var result =await _accountService.ChangeRole(changeRoleDto);
            return RedirectToAction("MemberRoles", new { changeRoleDto.Role });
        }

        [HttpPost("DeleteRoleByUserName")]
        public async Task<IActionResult> DeleteRoleByUserName(ChangeRoleDto changeRoleDto)
        {
            changeRoleDto.Add = false;
            var result = await _accountService.ChangeRole(changeRoleDto);
            return RedirectToAction("MemberRoles", new { changeRoleDto.Role });
        }

        [HttpGet("RegisterTerms")]
        public async Task<IActionResult> RegisterTerms()
        {
            List<EditRegisterTermDto> model = await _accountService.GetRegisterTermsList();
            return View(model);
        }

        [HttpPost("AddRegisterTerm")]
        public IActionResult AddRegisterTerm()
        {
            _accountService.AddRegisterTerm();
            return RedirectToAction("RegisterTerms");
        }

        [HttpGet("EditRegisterTerm")]
        public async Task<IActionResult> EditRegisterTerm(Guid RegisterTermId)
        {
            EditRegisterTermDto registerTerm = await _accountService.GetRegisterTermById(RegisterTermId);
            return View("EditRegisterTerm", registerTerm);
        }

        [HttpPost("EditRegisterTerm")]
        public async Task<IActionResult> EditRegisterTerm(EditRegisterTermDto editRegisterTermDto)
        {
            await _accountService.UpdateRegisterTerm(editRegisterTermDto);
            return RedirectToAction("RegisterTerms");
        }

        [HttpPost("SwitchRegisterTermEnabled")]
        public async Task<IActionResult> SwitchRegisterTermEnabled(Guid RegisterTermId)
        {
            await _accountService.SwitchRegisterTermEnabled(RegisterTermId);
            return RedirectToAction("RegisterTerms");
        }

        
        [HttpGet("Activities")]
        public async Task<IActionResult> Activities()
        {
            List<ActivityInfoDto> model = await _activityService.GetActivityInfos();
            return View(model);
        }
        
        [HttpGet("AddActivity")]
        public IActionResult AddActivity()
        {
            ActivitiyViewModel activities = new ActivitiyViewModel
            {
                ActivityDataDto = new ActivityDataDto()
                {
                    Id = Guid.NewGuid(),
                    RegistrationStartDateTime = DateTime.Now,
                    RegistrationEndDateTime = DateTime.Now,
                    StartDateTime = DateTime.Now,
                    EndDateTime = DateTime.Now,
                    CreatedDateTime = DateTime.Now,
                    CreatedUserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier),
                }
            };
            return View("EditActivity", activities);
        }
        
        [HttpGet("EditActivityById")]
        public async Task<IActionResult> EditActivityById(Guid ActivityId)
        {
            ActivitiyViewModel activities = await _activityService.GetActivitiesByActivityId(ActivityId);
            return View("EditActivity", activities);
        }

        [HttpPost("SwitchActivityEnabledById")]
        public async Task<IActionResult> SwitchActivityEnabledById(Guid ActivityId)
        {
            await _activityService.SwitchActivityEnabledById(ActivityId);
            return RedirectToAction("Activities");
        }

        [HttpPost("ActivityDataDto")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditActivityData(ActivityDataDto activityData)
        {
            if (await _activityService.GetActivityDataByActivityId(activityData.Id) != null)
            {
                activityData.UpdatedDateTime = DateTime.Now;
                activityData.UpdatedUserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                activityData.Id = await _activityService.UpdateActivityData(activityData);
                return RedirectToAction("EditActivityById", new { ActivityId = activityData.Id });
            }
            else if (activityData.Name != null)
            {
                activityData.CreatedDateTime = DateTime.Now;
                activityData.CreatedUserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                activityData.Id = await _activityService.UpdateActivityData(activityData);
                return RedirectToAction("EditActivityById", new { ActivityId = activityData.Id });
            }
            else
            {
                return View("EditActivity", new ActivitiyViewModel() { ActivityDataDto = activityData });
            }
        }

        [HttpPost("UploadActivityImages")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadActivityImages(Guid ActivityId, List<IFormFile> images)
        {
            if (images == null || images.Count == 0)
            {
                return BadRequest("No files received.");
            }
            await _activityService.UploadActivityImages(ActivityId, images);
            return Ok();
        }

        [HttpPost("EditActivityImage")]
        public async Task<IActionResult> EditActivityImage(ActivityImageDto ActivityImage)
        {
            if (ModelState.IsValid)
            {
                await _activityService.UpdateActivityImage(ActivityImage);
            }

            return RedirectToAction("EditActivityById", new {ActivityImage.ActivityId });
        }

        [HttpPost("DeleteActivityImageById")]
        public async Task<IActionResult> DeleteActivityImageById(Guid ActivityId, Guid ActivityImageId)
        {
            if (ModelState.IsValid)
            {
                await _activityService.DeleteActivityImageById(ActivityImageId);
            }

            return RedirectToAction("EditActivityById", new {ActivityId });
        }

        [HttpPost("SwitchActivityImageIsCoverById")]
        public async Task<IActionResult> SwitchActivityImageIsCoverById(Guid ActivityId, Guid ActivityImageId)
        {
            await _activityService.SwitchActivityImageIsCoverById(ActivityImageId);

            return RedirectToAction("EditActivityById", new {ActivityId });
        }

        [HttpPost("UploadActivityFiles")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadActivityFiles(Guid ActivityId, List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
            {
                return BadRequest("No files received.");
            }
            await _activityService.UploadActivityFiles(ActivityId, files);
            return RedirectToAction("EditActivityById", new {ActivityId });
        }

        [HttpPost("EditActivityFile")]
        public async Task<IActionResult> EditActivityFile(ActivityFileDto ActivityFile)
        {
            if (ModelState.IsValid)
            {
                await _activityService.UpdateActivityFile(ActivityFile);
            }

            return RedirectToAction("EditActivityById", new {ActivityFile.ActivityId });
        }

        [HttpPost("DeleteActivityFileById")]
        public async Task<IActionResult> DeleteActivityFileById(Guid ActivityId, Guid ActivityFileId)
        {
            if (ModelState.IsValid)
            {
                await _activityService.DeleteActivityFileById(ActivityFileId);
            }

            return RedirectToAction("EditActivityById", new {ActivityId });
        }
    }
}