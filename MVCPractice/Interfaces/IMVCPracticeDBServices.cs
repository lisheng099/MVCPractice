using MVCPractice.Dtos.Account;
using MVCPractice.Models.Activities;
using MVCPractice.ViewModels.Account;

namespace MVCPractice.Interfaces
{
    public interface IMVCPracticeDBServices
    {
        List<RegisterTermDto> GetRegisterTermDtoList();

        List<ActivityCategory> GetActivityCategories();

    }
}
