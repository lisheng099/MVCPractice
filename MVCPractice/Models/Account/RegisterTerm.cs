using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCPractice.Models.Account
{
    public class RegisterTerm
    {
        [Key]
        public Guid Id { get; set; }

        public int OrderIndex { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Content { get; set; } = "";

        public bool Enabled { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime UpdatedDateTime { get; set; }
    }
}