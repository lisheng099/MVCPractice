namespace MVCPractice.Dtos.Activities
{
    public class ActivityCategoryDto
    {
        public Guid Id { get; set; }
        public int OrderIndex { get; set; }
        public string Name { get; set; } = null!;
    }
}