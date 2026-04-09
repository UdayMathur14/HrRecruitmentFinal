namespace Models.RequestModels.Masters.Job
{
    public class JobSearchRequestModel
    {
        public Guid? DeptId { get; set; }
        public string? JobName { get; set; }
        public string? Status { get; set; }
        public string? JobStage { get; set; }
        public Guid? JobOwnerId { get; set; }
    }
}
