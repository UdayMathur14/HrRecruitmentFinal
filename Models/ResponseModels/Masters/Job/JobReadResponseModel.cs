namespace Models.ResponseModels.Masters.Job
{
    public class JobReadResponseModel
    {
        public Guid Id { get; set; }
        public Guid DeptId { get; set; }
        public string JobName { get; set; }
        public string? Description { get; set; }
        public int HeadCount { get; set; }
        public string? Status { get; set; }
        public string? JobStage { get; set; }
        public JobMemberReadResponseModel? Owner { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? ModifiedBy { get; set; }
        public List<JobMemberReadResponseModel> JobMembers { get; set; } = new();
    }
}
