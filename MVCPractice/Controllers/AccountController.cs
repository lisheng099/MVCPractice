using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using MVCPractice.Areas.Identity.Data;
using MVCPractice.Dtos.Account;
using MVCPractice.Interfaces;
using MVCPractice.Models.Account;
using MVCPractice.servers;
using MVCPractice.ViewModels.Account;
using System.Drawing;
using System.Net;

namespace MVCPractice.Controllers
{
    public class AccountController(
        ILogger<AccountController> logger,
        UserManager<MVCPracticeUser> userManager,
        RoleManager<IdentityRole> roleManager,
        SignInManager<MVCPracticeUser> signInManager,
        IMVCPracticeDBServices MVCPracticeDBServices,
        IAccountViewModelService accountViewModelService) : Controller
    {
        private readonly ILogger<AccountController> _logger = logger;
        private readonly UserManager<MVCPracticeUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly SignInManager<MVCPracticeUser> _signInManager = signInManager;
        private readonly IMVCPracticeDBServices _MVCPracticeDBServices = MVCPracticeDBServices;
        private readonly IAccountViewModelService _accountViewModelService = accountViewModelService;


        public IActionResult Index()
        {
            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }

        [HttpGet]
        public IActionResult Register()
        {
            List<RegisterTermDto> registerTermDtoList = _MVCPracticeDBServices.GetRegisterTermDtoList();
            RegisterViewModel registerViewModel = _accountViewModelService.GetRegisterViewModel(registerTermDtoList);

            return View(registerViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid) {
                var user = new MVCPracticeUser
                {
                    UserName = model.Username,
                    Email = model?.Email,
                    Name = model.Name,
                    Birthday = model?.Birthday,
                    Gender = model?.Gender,
                    IdNumber = model.IdNumber,
                    Profession = model?.Profession,
                    Education = model?.Education,
                    Marriage = model?.Marriage,
                    Religion = model?.Religion,
                    PhoneNumber = model?.Phone,
                    LandlinePhone = model?.LandlinePhone,
                    Address = model?.Address
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(model);
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(model.Username);
                    var roles = await _userManager.GetRolesAsync(user);
                    HttpContext.Session.SetString("UserRoles", string.Join(",", roles));

                    _logger.LogInformation("User logged in.");

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "登入失敗。");
                    return View(model);
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Logout()
        {
            _signInManager.SignOutAsync();
            HttpContext.Session.Remove("UserRoles");
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            MVCPracticeUser user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            HttpContext.Session.SetString("UserName", user.UserName);

            EditProfileViewModel editProfileViewModel = 
                new EditProfileViewModel()
                {
                    UserName = user.UserName,
                    Name=user.Name,
                    Birthday=user.Birthday,
                    Gender=user.Gender,
                    IdNumber=user.IdNumber,
                    Profession=user.Profession,
                    Education=user.Education,
                    Marriage=user.Marriage,
                    Religion=user.Religion,
                    LandlinePhone=user.LandlinePhone,
                    Address=user.Address,
                    PhoneNumber=user.PhoneNumber,
                    Email=user.Email,
                };
            return View(editProfileViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> EditProfile(EditProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null || user.UserName != model.UserName)
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

                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditPassword()
        {
            MVCPracticeUser user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            EditPasswordViewModel editPasswordViewModel =
                new EditPasswordViewModel()
                {
                    UserName = user.UserName
                };
            return View(editPasswordViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPassword(EditPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                MVCPracticeUser user = await _userManager.GetUserAsync(User);

                if (user == null)
                {
                    return NotFound("找不到該使用者");
                }
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.OldPassword, false, false);
                if (!result.Succeeded)
                {
                    return BadRequest("與當前密碼不符");
                }
                var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.Password);
                if (!changePasswordResult.Succeeded)
                {
                    return BadRequest(changePasswordResult.Errors);
                }
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
