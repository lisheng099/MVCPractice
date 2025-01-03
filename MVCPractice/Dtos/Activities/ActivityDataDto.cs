namespace MVCPractice.Dtos.Activities
{
    public class ActivityDataDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int? OrderIndex { get; set; }
        public int? CategoryId { get; set; }
        public int? PersonsNumber { get; set; }
        public string Introduce { get; set; }
        public string Content { get; set; }
        public bool Enabled { get; set; }
        public DateTime RegistrationStartDateTime { get; set; }
        public DateTime RegistrationEndDateTime { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string CreatedUserName { get; set; }
        public DateTime UpdatedDateTime { get; set; }
        public string UpdatedUserName { get; set; }
    }
}
