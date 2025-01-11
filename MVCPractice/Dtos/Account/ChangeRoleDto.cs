using System.ComponentModel.DataAnnotations;

namespace MVCPractice.Dtos.Account
{
    public class ChangeRoleDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public bool Add { get; set; }

        [Required]
        public string Role { get; set; }
    }
}