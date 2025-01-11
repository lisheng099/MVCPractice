using System.ComponentModel.DataAnnotations;

namespace MVCPractice.ViewModels.Account
{
    public class RegisterTermViewModel
    {
        public int OrderIndex { get; set; }
        public string TermTexts { get; set; }

        [Required(ErrorMessage = "請同意條款")]
        [Display(Name = "我已閱讀並同意條款")]
        public bool AgreeToTerms { get; set; }
    }
}