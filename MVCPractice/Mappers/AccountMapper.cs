using MVCPractice.Dtos.Account;
using MVCPractice.Dtos.Sysadm;
using MVCPractice.Models.Account;
using MVCPractice.ViewModels.Account;

namespace MVCPractice.Mappers
{
    public static class AccountMapper
    {
        public static EditRegisterTermDto RegisterTermDataToEditDto(this RegisterTerm value)
        {
            return new EditRegisterTermDto
            {
                Id = value.Id,
                OrderIndex = value.OrderIndex,
                Content = value.Content,
                Enabled = value.Enabled,
                CreatedDateTime = value.CreatedDateTime,
                UpdatedDateTime = value.UpdatedDateTime
            };
        }

        public static ApplicationUser RegisterViewModelToUser(this RegisterViewModel value)
        {
            return new ApplicationUser
            {
                UserName = value.Username,
                Email = value.Email,
                Name = value.Name,
                Birthday = value.Birthday,
                Gender = value.Gender,
                IdNumber = value.IdNumber,
                Profession = value.Profession,
                Education = value.Education,
                Marriage = value.Marriage,
                Religion = value.Religion,
                PhoneNumber = value.Phone,
                LandlinePhone = value.LandlinePhone,
                Address = value.Address
            };
        }

        public static EditProfileViewModel UserToEditProfileViewModel(this ApplicationUser value)
        {
            return new EditProfileViewModel()
            {
                Id = value.Id,
                UserName = value.UserName,
                Name = value.Name,
                Birthday = value.Birthday,
                Gender = value.Gender,
                IdNumber = value.IdNumber,
                Profession = value.Profession,
                Education = value.Education,
                Marriage = value.Marriage,
                Religion = value.Religion,
                LandlinePhone = value.LandlinePhone,
                Address = value.Address,
                PhoneNumber = value.PhoneNumber,
                Email = value.Email,
            };
        }

        public static UserDto UserToUserDto(this ApplicationUser value)
        {
            return new UserDto()
            {
                Id = value.Id,
                UserName = value.UserName,
                Name = value.Name
            };
        }
    }
}