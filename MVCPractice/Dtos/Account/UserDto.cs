using System.ComponentModel.DataAnnotations;

namespace MVCPractice.Dtos.Account
{
    public class UserDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
    }
}