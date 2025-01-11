using MVCPractice.Dtos.Account;

namespace MVCPractice.ViewModels.Sysadm
{
    public class MemberRolesViewModel
    {
        public string SearchTerm { get; set; } = null;
        public IEnumerable<UserDto> Users { get; set; }
    }
}