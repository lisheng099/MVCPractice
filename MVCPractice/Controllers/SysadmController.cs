using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCPractice.Areas.Identity.Data;
using MVCPractice.ViewModels.Account;
using MVCPractice.ViewModels.Sysadm;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MVCPractice.Controllers
{
    public class SysadmController(ILogger<AccountController> logger,
        UserManager<MVCPracticeUser> userManager,
        RoleManager<IdentityRole> roleManager) : Controller
    {
        private readonly UserManager<MVCPracticeUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;

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
    }
}
