namespace MVCPractice.Dtos.Activities
{
    public class ActivityFileDto
    {
        public Guid Id { get; set; }
        public Guid ActivityId { get; set; }
        public string Name { get; set; }
        public int OrderIndex { get; set; }
        public string Url { get; set; }
    }
}