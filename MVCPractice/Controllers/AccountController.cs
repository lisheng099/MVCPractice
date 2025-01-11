using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCPractice.Attributes;
using MVCPractice.Dtos.Account;
using MVCPractice.Interfaces;
using MVCPractice.ViewModels.Account;

namespace MVCPractice.Controllers
{
    [Route("[controller]")]
    [AllowAnonymous]
    public class AccountController(
        IAccountService accountService) : Controller
    {
        private readonly IAccountService _accountService = accountService;

        [HttpGet("Register")]
        public async Task<IActionResult> Register()
        {
            RegisterViewModel registerViewModel = await _accountService.GetRegisterViewModel();

            return View(registerViewModel);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountService.Register(model);
                if (result.Succeeded)
                {
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

        [HttpGet("Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("Login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginPostDto model)
        {
            if (ModelState.IsValid)
            {
                var principal = await _accountService.Login(model);
                if (principal != null)
                {
                    await HttpContext.SignInAsync(principal);
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

        [HttpGet("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _accountService.Logout();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet("EditProfile/{Id}")]
        [ValidateUserId]
        public async Task<IActionResult> EditProfile(String Id)
        {
            EditProfileViewModel editProfileViewModel = await _accountService.GetEditProfileViewModel(Id);
            if (editProfileViewModel == null) { return NotFound("找不到該會員或沒有權限修改。"); }
            return View(editProfileViewModel);
        }

        [HttpPost("EditProfile/{Id}")]
        [ValidateUserId]
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

        [HttpGet("EditPassword/{Id}")]
        [ValidateUserId]
        public async Task<IActionResult> EditPassword(String Id)
        {
            EditPasswordViewModel model = await _accountService.GetEditPasswordViewModel(Id);
            return View(model);
        }

        [HttpPost("EditPassword/{Id}")]
        [ValidateUserId]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPassword(String Id, EditPasswordViewModel model)
        {
            model.Id = Id;
            if (ModelState.IsValid)
            {
                var result = await _accountService.EditPassword(model);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }
    }
}