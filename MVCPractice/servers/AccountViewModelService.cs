using MVCPractice.Dtos.Account;
using MVCPractice.Interfaces;
using MVCPractice.Models;
using MVCPractice.ViewModels.Account;

namespace MVCPractice.servers
{
    public class AccountViewModelService : IAccountViewModelService
    {
        public RegisterViewModel GetRegisterViewModel(List<RegisterTermDto> registerTermDtos)
        {
            List<RegisterTermViewModel> registerTermViewModels = (from a in registerTermDtos
                                                                  orderby a.OrderIndex
                                                                  select new RegisterTermViewModel
                                                                  {
                                                                      OrderIndex = a.OrderIndex,
                                                                      TermTexts = a.Content,
                                                                      AgreeToTerms = false
                                                                  }).ToList();

            RegisterViewModel registerViewModel = new RegisterViewModel() { registerTermViewModel = registerTermViewModels };

            return registerViewModel;
        }

    }
}
