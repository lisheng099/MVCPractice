using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCPractice.Areas.Identity.Data;
using MVCPractice.Dtos.Activities;
using MVCPractice.Dtos.Sysadm;
using MVCPractice.Interfaces;
using MVCPractice.ViewModels.Account;
using MVCPractice.ViewModels.Activities;
using MVCPractice.ViewModels.Sysadm;
using System.Data;

namespace MVCPractice.Controllers
{
    public class SysadmController(ILogger<AccountController> logger,
        UserManager<MVCPracticeUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IAccountService accountViewModelService,
        IActivityService activityService) : Controller
    {
        private readonly UserManager<MVCPracticeUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly IAccountService _accountService = accountViewModelService;
        private readonly IActivityService _activityService = activityService;

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }

        [HttpGet]
        public async Task<IActionResult> Members(string searchTerm)
        {
            IEnumerable<MembersViewModel> membersViewModels;
            if (!string.IsNullOrEmpty(searchTerm)) 
            {
                membersViewModels = await _userManager.Users
               .Where(u => u.UserName.Contains(searchTerm) || u.Name.Contains(searchTerm))
               .Select(a => new MembersViewModel() { UserName = a.UserName, Name = a.Name }).ToListAsync();
            }
            else
            {
                membersViewModels = await _userManager.Users
               .Select(a => new MembersViewModel() { UserName = a.UserName, Name = a.Name }).ToListAsync();
            }

            ViewData["searchTerm"] = searchTerm ;

            return View(membersViewModels);
        }

        [HttpGet]
        public async Task<IActionResult> EditProfile(String UserName = null)
        {
            MVCPracticeUser user = await _userManager.FindByNameAsync(UserName);
            if (user == null)
            {
                return NotFound();
            }

            HttpContext.Session.SetString("UserName", user.UserName);

            EditProfileViewModel editProfileViewModel =
                new EditProfileViewModel()
                {
                    UserName = user.UserName,
                    Name = user.Name,
                    Birthday = user.Birthday,
                    Gender = user.Gender,
                    IdNumber = user.IdNumber,
                    Profession = user.Profession,
                    Education = user.Education,
                    Marriage = user.Marriage,
                    Religion = user.Religion,
                    LandlinePhone = user.LandlinePhone,
                    Address = user.Address,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email,
                };
            return View(editProfileViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(EditProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user != null)
                {
                    user.Email = model.Email;
                    user.Name = model.Name;
                    user.Birthday = model.Birthday;
                    user.Gender = model.Gender;
                    user.IdNumber = model.IdNumber;
                    user.Profession = model.Profession;
                    user.Education = model.Education;
                    user.Marriage = model.Marriage;
                    user.Religion = model.Religion;
                    user.PhoneNumber = model.PhoneNumber;
                    user.LandlinePhone = model.LandlinePhone;
                    user.Address = model.Address;

                    var result = await _userManager.UpdateAsync(user);
                    if (!result.Succeeded)
                    {
                        return View(model);
                    }

                    return RedirectToAction("Members");
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(String UserName = null)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(UserName);
                if (user == null)
                {
                    return NotFound("找不到該使用者");
                }
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var changePasswordResult = await _userManager.ResetPasswordAsync(user, token, "Az0.123");
                if (!changePasswordResult.Succeeded)
                {
                    return BadRequest(changePasswordResult.Errors);
                }
            }
            return RedirectToAction("Members");
        }

        [HttpGet]
        public async Task<IActionResult> MemberRoles(string Role)
        {
            IEnumerable<MemberRolesViewModel> memberRolesViewModels;
            if (string.IsNullOrEmpty(Role))
            {
                Role = _roleManager.Roles.Select(r => r.Name).First();
            }

            memberRolesViewModels = (await _userManager.GetUsersInRoleAsync(Role))
                .Select(a => new MemberRolesViewModel() { UserName = a.UserName, Name = a.Name });

            ViewData["CurrentRole"] = Role;

            return View(memberRolesViewModels);
        }

        [HttpPost]
        public async Task<IActionResult> AddRoleByUserName(String Role, String UserName)
        {
            if (UserName == null)
            {
                return NotFound("使用者輸入為空");
            }
            var user = await _userManager.FindByNameAsync(UserName);

            if (user == null)
            {
                return NotFound("找不到該使用者");
            }
            else
            {
                await _userManager.AddToRoleAsync(user, Role);
            }

            ViewData["CurrentRole"] = Role;

            var usersInRole = await _userManager.GetUsersInRoleAsync(Role);
            return RedirectToAction("MemberRoles", new { Role });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRoleByUserName(String Role, String UserName)
        {
            var user = await _userManager.FindByNameAsync(UserName);
            await _userManager.RemoveFromRoleAsync(user, Role);

            return RedirectToAction("MemberRoles", new { Role });
        }

        [HttpGet]
        public IActionResult RegisterTerms()
        {
            List<EditRegisterTermDto> model = _accountService.GetRegisterTermsList();
            return View(model);
        }

        [HttpPost]
        public IActionResult AddRegisterTerm()
        {
            _accountService.AddRegisterTerm();
            return RedirectToAction("RegisterTerms");
        }

        [HttpGet]
        public IActionResult EditRegisterTerm(int RegisterTermId)
        {
            EditRegisterTermDto registerTerm = _accountService.GetRegisterTermById(RegisterTermId);
            return View("EditRegisterTerm", registerTerm);
        }

        [HttpPost]
        public IActionResult EditRegisterTerm(EditRegisterTermDto editRegisterTermDto)
        {
            _accountService.UpdateRegisterTerm(editRegisterTermDto);
            return RedirectToAction("RegisterTerms");
        }

        [HttpPost]
        public IActionResult SwitchRegisterTermEnabled(int RegisterTermId)
        {
            _accountService.SwitchRegisterTermEnabled(RegisterTermId);
            return RedirectToAction("RegisterTerms");
        }

        [HttpGet]
        public IActionResult Activities()
        {
            List<ActivityInfoDto> model = _activityService.GetActivityInfos();
            return View(model);
        }

        [HttpGet]
        public IActionResult AddActivity()
        {
            ActivitiyViewModel activities = new ActivitiyViewModel
            {
                ActivityDataDto = new ActivityDataDto()
                {
                    Id = -1,
                    RegistrationStartDateTime = DateTime.Now,
                    RegistrationEndDateTime = DateTime.Now,
                    StartDateTime = DateTime.Now,
                    EndDateTime = DateTime.Now,
                    CreatedDateTime = DateTime.Now,
                    CreatedUserName = User.Identity.Name
                }
            };
            return View("EditActivity", activities);
        }

        [HttpGet]
        public IActionResult EditActivityById(int ActivityId)
        {
            ActivitiyViewModel activities = _activityService.GetActivitiesByActivityId(ActivityId);
            return View("EditActivity", activities);
        }

        [HttpPost]
        public IActionResult SwitchActivityEnabledById(int ActivityId)
        {
            _activityService.SwitchActivityEnabledById(ActivityId);
            return RedirectToAction("Activities");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditActivityData(ActivityDataDto activityData)
        {
            if (_activityService.GetActivityDataByActivityId(activityData.Id) != null)
            {
                activityData.UpdatedDateTime = DateTime.Now;
                activityData.UpdatedUserName = _userManager.GetUserName(User);
                _activityService.UpdateActivityData(activityData);
                return RedirectToAction("EditActivityById", new { ActivityId = activityData.Id });
            }
            else if (activityData.Name != null)
            {
                activityData.CreatedDateTime = DateTime.Now;
                activityData.CreatedUserName = _userManager.GetUserName(User);
                activityData.Id = _activityService.UpdateActivityData(activityData);
                return RedirectToAction("EditActivityById", new { ActivityId = activityData.Id });
            }
            else
            {
                return View("EditActivity", new ActivitiyViewModel() { ActivityDataDto = activityData });
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadActivityImages(int ActivityId, List<IFormFile> images)
        {
            if (images == null || images.Count == 0)
            {
                return BadRequest("No files received.");
            }
            await UploadActivityImages(ActivityId, images);
            return Ok();
        }

        [HttpPost]
        public IActionResult EditActivityImage(ActivityImageDto ActivityImage)
        {
            if (ModelState.IsValid)
            {
                _activityService.UpdateActivityImage(ActivityImage);
            }

            return RedirectToAction("EditActivityById", new { ActivityId = ActivityImage.ActivityId });
        }

        [HttpPost]
        public IActionResult DeleteActivityImageById(int ActivityId, int ActivityImageId)
        {
            if (ModelState.IsValid)
            {
                _activityService.DeleteActivityImageById(ActivityImageId);
            }

            return RedirectToAction("EditActivityById", new { ActivityId = ActivityId });
        }

        [HttpPost]
        public IActionResult SwitchActivityImageIsCoverById(int ActivityId, int ActivityImageId)
        {
            _activityService.SwitchActivityImageIsCoverById(ActivityImageId);

            return RedirectToAction("EditActivityById", new { ActivityId = ActivityId });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UploadActivityFiles(int ActivityId, List<IFormFile> files)
        {

            if (files == null || files.Count == 0)
            {
                return BadRequest("No files received.");
            }
            _activityService.UploadActivityFiles(ActivityId, files);
            return RedirectToAction("EditActivityById", new { ActivityId = ActivityId });
        }

        [HttpPost]
        public IActionResult EditActivityFile(ActivityFileDto ActivityFile)
        {
            if (ModelState.IsValid)
            {
                _activityService.UpdateActivityFile(ActivityFile);
            }

            return RedirectToAction("EditActivityById", new { ActivityId = ActivityFile.ActivityId });
        }

        [HttpPost]
        public IActionResult DeleteActivityFileById(int ActivityId, int ActivityFileId)
        {
            if (ModelState.IsValid)
            {
                _activityService.DeleteActivityFileById(ActivityFileId);
            }

            return RedirectToAction("EditActivityById", new { ActivityId = ActivityId });
        }
    }
}
