using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MVCPractice.Dtos.Account;
using MVCPractice.Dtos.Sysadm;
using MVCPractice.Interfaces;
using MVCPractice.Mappers;
using MVCPractice.Models;
using MVCPractice.Models.Account;
using MVCPractice.ViewModels.Account;
using MVCPractice.ViewModels.Sysadm;
using System.Data;
using System.Security.Claims;

namespace MVCPractice.services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly MVCPracticeDBContext _dbContext;

        public AccountService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            MVCPracticeDBContext mVCPracticeDBContext
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _dbContext = mVCPracticeDBContext;
        }

        public async Task<ClaimsPrincipal> Login(LoginPostDto value)
        {
            var SignIn = await _signInManager.PasswordSignInAsync(value.Username, value.Password, true, true);
            if (SignIn.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(value.Username);
                var roles = await _userManager.GetRolesAsync(user);

                var claims = new List<Claim>
                {
                    new(ClaimTypes.NameIdentifier, user.Id),
                    new(ClaimTypes.Name, user.UserName),
                };

                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var identity = new ClaimsIdentity(claims, "apiauth");
                var principal = new ClaimsPrincipal(identity);

                return principal;
            }
            else
            {
                return null;
            }
        }
        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }
        public async Task<RegisterViewModel> GetRegisterViewModel()
        {
            List<RegisterTermViewModel> registerTermViewModels = await (from a in _dbContext.RegisterTerms
                                                                        where a.Enabled
                                                                        orderby a.OrderIndex
                                                                        select new RegisterTermViewModel
                                                                        {
                                                                            OrderIndex = a.OrderIndex,
                                                                            TermTexts = a.Content,
                                                                            AgreeToTerms = false
                                                                        }).ToListAsync();

            RegisterViewModel registerViewModel = new RegisterViewModel() { registerTermViewModel = registerTermViewModels };

            return registerViewModel;
        }

        public async Task<IdentityResult> Register(RegisterViewModel value)
        {
            var user = value.RegisterViewModelToUser();

            var result = await _userManager.CreateAsync(user, value.Password);
            if (result.Succeeded)
            {
                var ChangeRoleResult = await ChangeRole(new ChangeRoleDto { UserName = user.UserName, Add = true, Role = "User" });
                if (!ChangeRoleResult.Succeeded)
                {
                    return ChangeRoleResult;
                }
            }
            return result;
        }

        public async Task<EditProfileViewModel> GetEditProfileViewModel(String Id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(Id);
            if (user == null)
            {
                return null;
            }
            EditProfileViewModel result = user.UserToEditProfileViewModel();
            return result;
        }

        public async Task<IdentityResult> EditProfile(EditProfileViewModel value)
        {
            var user = await _userManager.FindByIdAsync(value.Id);
            if (user == null)
            {
                return null;
            }
            user.Email = value.Email;
            user.Birthday = value.Birthday;
            user.Gender = value.Gender;
            user.IdNumber = value.IdNumber;
            user.Profession = value.Profession;
            user.Education = value.Education;
            user.Marriage = value.Marriage;
            user.Religion = value.Religion;
            user.PhoneNumber = value.PhoneNumber;
            user.LandlinePhone = value.LandlinePhone;
            user.Address = value.Address;

            var result = await _userManager.UpdateAsync(user);

            return result;
        }

        public async Task<EditPasswordViewModel> GetEditPasswordViewModel(String Id)
        {
            var user = await _userManager.FindByIdAsync(Id);
            if (user == null) { return null; }
            return new EditPasswordViewModel()
            {
                UserName = user.UserName
            };
        }
        public async Task<IdentityResult> EditPassword(EditPasswordViewModel model)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                return null;
            }
            var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.Password);
            return changePasswordResult;
        }

        public async Task<MembersViewModel> GetMembersViewModel(string searchTerm = null)
        {
            var Users = _userManager.Users;
            if (searchTerm != null)
            {
                Users = Users.Where(x => x.UserName.Contains(searchTerm) || x.Name.Contains(searchTerm));
            }
            MembersViewModel result = new MembersViewModel()
            {
                SearchTerm = searchTerm,
                Users = await Users.Select(x => x.UserToUserDto()).ToListAsync(),
            };

            return result;
        }

        public async Task<MemberRolesViewModel> GetMemberRolesViewModel(string searchTerm = null)
        {

            if (string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = _roleManager.Roles.Select(r => r.Name).First();
            }
            var memberRolesViewModels = await _userManager.GetUsersInRoleAsync(searchTerm);

            MemberRolesViewModel result = new MemberRolesViewModel()
            {
                SearchTerm = searchTerm,
                Users = (await _userManager.GetUsersInRoleAsync(searchTerm)).Select(x => x.UserToUserDto()).ToList(),
            };

            return result;
        }

        public async Task<IdentityResult> ResetPassword(String Id)
        {
            var user = await _userManager.FindByIdAsync(Id);
            if (user == null)
            {
                return null;
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, "Aa111111?");
            return result;
        }

        public async Task<IdentityResult> ChangeRole(ChangeRoleDto value)
        {
            var user = await _userManager.FindByNameAsync(value.UserName);
            IdentityResult result;
            if (user == null || !await _roleManager.RoleExistsAsync(value.Role))
            {
                return null;
            }
            if (value.Add)
            {
                result = await _userManager.AddToRoleAsync(user, value.Role);
            }
            else
            {
                result = await _userManager.RemoveFromRoleAsync(user, value.Role);
            }
            return result;
        }

        public async Task<List<EditRegisterTermDto>> GetRegisterTermsList()
        {
            List<EditRegisterTermDto> registerTerms = await (from a in _dbContext.RegisterTerms
                                                             select a.RegisterTermDataToEditDto()).ToListAsync();
            return registerTerms;
        }

        public async Task AddRegisterTerm()
        {
            RegisterTerm registerTerm = new RegisterTerm();
            _dbContext.RegisterTerms.Add(registerTerm);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<EditRegisterTermDto> GetRegisterTermById(Guid Id)
        {
            List<EditRegisterTermDto> registerTerms = await (from a in _dbContext.RegisterTerms
                                                             where a.Id == Id
                                                             select a.RegisterTermDataToEditDto()).ToListAsync();

            if (registerTerms.Count != 0)
            {
                EditRegisterTermDto registerTerm = registerTerms.First();
                return registerTerm;
            }
            else
            {
                RegisterTerm registerTerm = new RegisterTerm();
                _dbContext.RegisterTerms.Add(registerTerm);
                _dbContext.SaveChanges();

                return registerTerm.RegisterTermDataToEditDto();
            }
        }

        public async Task UpdateRegisterTerm(EditRegisterTermDto registerTerm)
        {
            var registerTerms = (from a in _dbContext.RegisterTerms
                                 where a.Id == registerTerm.Id
                                 select a);
            if (registerTerms.Any())
            {
                RegisterTerm oldRegisterTerm = registerTerms.First();
                oldRegisterTerm.OrderIndex = 0;
                oldRegisterTerm.Content = registerTerm.Content;
                oldRegisterTerm.Enabled = registerTerm.Enabled;
                oldRegisterTerm.UpdatedDateTime = DateTime.Now;
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task SwitchRegisterTermEnabled(Guid registerTermId)
        {
            List<RegisterTerm> registerTerms = (from a in _dbContext.RegisterTerms
                                                where a.Id == registerTermId
                                                select a).ToList();
            if (registerTerms.Count != 0)
            {
                RegisterTerm registerTerm = registerTerms.First();
                registerTerm.Enabled = !registerTerm.Enabled;
                registerTerm.UpdatedDateTime = DateTime.Now;
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}