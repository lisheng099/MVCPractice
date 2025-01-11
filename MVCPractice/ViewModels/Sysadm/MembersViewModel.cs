using MVCPractice.Dtos.Account;

namespace MVCPractice.ViewModels.Sysadm
{
    public class MembersViewModel
    {
        public string SearchTerm { get; set; } = null;
        public IEnumerable<UserDto> Users { get; set; }
    }
}