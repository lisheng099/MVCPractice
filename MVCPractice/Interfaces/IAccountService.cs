using MVCPractice.Dtos.Account;
using MVCPractice.Dtos.Activities;
using MVCPractice.Dtos.Sysadm;
using MVCPractice.ViewModels.Account;

namespace MVCPractice.Interfaces
{
    public interface IAccountService
    {

        List<EditRegisterTermDto> GetRegisterTermsList();

        void AddRegisterTerm();
        EditRegisterTermDto GetRegisterTermById(int Id);
        void UpdateRegisterTerm(EditRegisterTermDto registerTerm);
        void SwitchRegisterTermEnabled(int registerTermId);

        RegisterViewModel GetRegisterViewModel();
    }
}
