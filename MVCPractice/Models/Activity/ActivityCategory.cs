namespace MVCPractice.Models.Activities
{
    public class ActivityCategory
    {
        public Guid Id { get; set; }
        public int OrderIndex { get; set; }
        public string Name { get; set; }
        public virtual ICollection<ActivityData> ActivityDatas { get; set; }
    }
}