using System.ComponentModel.DataAnnotations;

namespace MVCPractice.Dtos.Account
{
    public class LoginPostDto
    {
        [Required(ErrorMessage = "請輸入帳號")]
        [Display(Name = "帳號")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "請輸入密碼")]
        [DataType(DataType.Password)]
        [Display(Name = "密碼")]
        public string Password { get; set; } = null!;
    }
}