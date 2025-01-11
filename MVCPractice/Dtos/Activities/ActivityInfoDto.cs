namespace MVCPractice.Dtos.Activities
{
    public class ActivityInfoDto
    {
        public Guid ActivityId { get; set; }
        public string Name { get; set; } = null!;
        public int OrderIndex { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int ParticipatedPersonsNumber { get; set; }
        public int PersonsNumber { get; set; }
        public string Introduce { get; set; }
        public string Content { get; set; }
        public bool Enabled { get; set; }
        public DateTime RegistrationEndDateTime { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }

        public bool Participated { get; set; }

        public string CoverImageUrl { get; set; } = null!;

        public List<ActivityImageDto> ActivityImages { get; set; } = null!;
        public List<ActivityFileDto> ActivityFiles { get; set; } = null!;
    }
}