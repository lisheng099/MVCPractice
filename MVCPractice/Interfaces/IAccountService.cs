using Microsoft.AspNetCore.Identity;
using MVCPractice.Dtos.Account;
using MVCPractice.Dtos.Sysadm;
using MVCPractice.ViewModels.Account;
using MVCPractice.ViewModels.Sysadm;
using System.Security.Claims;

namespace MVCPractice.Interfaces
{
    public interface IAccountService
    {
        public Task<ClaimsPrincipal> Login(LoginPostDto value);
        public Task Logout();

        public Task<RegisterViewModel> GetRegisterViewModel();
        public Task<IdentityResult> Register(RegisterViewModel value);

        public Task<EditProfileViewModel> GetEditProfileViewModel(String Id);
        public Task<IdentityResult> EditProfile(EditProfileViewModel value);

        public Task<EditPasswordViewModel> GetEditPasswordViewModel(String Id);
        public Task<IdentityResult> EditPassword(EditPasswordViewModel value);

        public Task<MembersViewModel> GetMembersViewModel(string searchTerm = null);

        public Task<MemberRolesViewModel> GetMemberRolesViewModel(string searchTerm = null);

        public Task<IdentityResult> ResetPassword(String Id);

        public Task<IdentityResult> ChangeRole(ChangeRoleDto value);

        public Task<List<EditRegisterTermDto>> GetRegisterTermsList();
        public Task<EditRegisterTermDto> GetRegisterTermById(Guid Id);
        public Task AddRegisterTerm();
        public Task UpdateRegisterTerm(EditRegisterTermDto registerTerm);
        public Task SwitchRegisterTermEnabled(Guid registerTermId);

    }
}