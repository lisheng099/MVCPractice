using System.ComponentModel.DataAnnotations;

namespace MVCPractice.Dtos.Account
{
    public class RegisterDto
    {
        [MaxLength(20)]
        [Display(Name = "姓名")]
        public string Name { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "生日")]
        public DateTime? Birthday { get; set; }

        [MaxLength(20)]
        [Display(Name = "性別")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "請輸入身分證字號")]
        [MaxLength(20)]
        [Display(Name = "身分證字號")]
        public string IdNumber { get; set; } = null!;

        [MaxLength(30)]
        [Display(Name = "職業")]
        public string Profession { get; set; }

        [MaxLength(20)]
        [Display(Name = "教育程度")]
        public string Education { get; set; }

        [MaxLength(20)]
        [Display(Name = "婚姻狀況")]
        public string Marriage { get; set; }

        [MaxLength(20)]
        [Display(Name = "宗教信仰")]
        public string Religion { get; set; }

        [Required(ErrorMessage = "請輸入手機號碼")]
        [Phone(ErrorMessage = "手機號碼格式不正確")]
        [Display(Name = "手機")]
        public string Phone { get; set; } = null!;

        [MaxLength(20)]
        [Display(Name = "室內電話")]
        public string LandlinePhone { get; set; }

        [Required(ErrorMessage = "請輸入電子郵件")]
        [EmailAddress(ErrorMessage = "電子郵件格式不正確")]
        [Display(Name = "電子郵件")]
        public string Email { get; set; } = null!;

        [MaxLength(100)]
        [Display(Name = "地址")]
        public string Address { get; set; }

        [Required(ErrorMessage = "請輸入帳號")]
        [StringLength(20, ErrorMessage = "{0} 長度必須介於 {2} 到 {1} 個字元之間。", MinimumLength = 6)]
        [Display(Name = "帳號")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "請輸入密碼")]
        [StringLength(100, ErrorMessage = "{0} 長度必須介於 {2} 到 {1} 個字元之間。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密碼")]
        public string Password { get; set; } = null!;
    }
}
