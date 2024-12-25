using Microsoft.EntityFrameworkCore;
using MVCPractice.Dtos.Account;
using MVCPractice.Models.Activities;
using MVCPractice.ViewModels.Account;

namespace MVCPractice.Interfaces
{
    public interface IAccountViewModelService
    {
        RegisterViewModel GetRegisterViewModel(List<RegisterTermDto> registerTermDtos);

    }
}
