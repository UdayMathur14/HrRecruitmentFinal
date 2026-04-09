namespace Models.RequestModels.Masters.Job
{
    public class JobUpdateRequestModel
    {
        public Guid? DeptId { get; set; }
        public string? JobName { get; set; }
        public string? Description { get; set; }
        public int? HeadCount { get; set; }
        public string? Status { get; set; }
        public string? JobStage { get; set; }
        public Guid? JobOwnerId { get; set; }
        public Guid? ActionBy { get; set; }
        public List<JobMemberRequestModel> JobMembers { get; set; } = new();
    }
}
