namespace MVCPractice.Dtos.Sysadm
{
    public class EditRegisterTermDto
    {
        public Guid Id { get; set; }
        public int OrderIndex { get; set; }
        public string Content { get; set; } = "";
        public bool Enabled { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime UpdatedDateTime { get; set; }
    }
}