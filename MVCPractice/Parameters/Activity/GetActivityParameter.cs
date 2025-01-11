namespace MVCPractice.Parameters.Activity
{
    public class GetActivityParameter
    {
        public Guid? ActivityId { get; set; }
        public Guid? CategoryId { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public String UserId { get; set; }
        public bool CheckIsParticipated { get; set; } = false;
    }
}
