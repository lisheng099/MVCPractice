using System.ComponentModel.DataAnnotations;

namespace MVCPractice.ViewModels.Account
{
    public class EditPasswordViewModel
    {
        [Required(ErrorMessage = "請輸入帳號")]
        [StringLength(20, ErrorMessage = "{0} 長度必須介於 {2} 到 {1} 個字元之間。", MinimumLength = 6)]
        [Display(Name = "帳號")]
        public string UserName { get; set; } = null!;

        [Required(ErrorMessage = "請輸入舊密碼")]
        [StringLength(100, ErrorMessage = "{0} 長度必須介於 {2} 到 {1} 個字元之間。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "舊密碼")]
        public string OldPassword { get; set; } = null!;

        [Required(ErrorMessage = "請輸入密碼")]
        [StringLength(100, ErrorMessage = "{0} 長度必須介於 {2} 到 {1} 個字元之間。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密碼")]
        public string Password { get; set; } = null!;

        [DataType(DataType.Password)]
        [Display(Name = "確認密碼")]
        [Compare("Password", ErrorMessage = "密碼和確認密碼不相符。")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
