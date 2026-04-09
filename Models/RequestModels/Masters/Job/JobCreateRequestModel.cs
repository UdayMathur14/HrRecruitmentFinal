namespace Models.RequestModels.Masters.Job
{
    public class JobCreateRequestModel
    {
        public Guid DeptId { get; set; }
        public string JobName { get; set; }
        public string? Description { get; set; }
        public int HeadCount { get; set; }
        public string? JobStage { get; set; }
        public Guid? JobOwnerId { get; set; }
        public List<JobMemberRequestModel> JobMembers { get; set; } = new();
        public Guid? CreatedBy { get; set; }
    }

    public class JobMemberRequestModel
    {
        public Guid UserId { get; set; }
    }
}
