namespace MVCPractice.Dtos.Activities
{
    public class ActivityImageDto
    {
        public int Id { get; set; }
        public int ActivityId { get; set; }
        public string Name { get; set; }
        public bool IsCover { get; set; }
        public int OrderIndex { get; set; }
        public string Url { get; set; }
    }
}
