namespace DataAccessLayer.Domain.Masters.DepartmentSummary
{
    public class DepartmentSummaryResponseEntity
    {
        public Guid DepartmentId { get; set; }
        public Guid? JobId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public string? DepartmentDescription { get; set; }
        public int TotalJobs { get; set; }
        public int ActivePositions { get; set; }
        public int TeamMembers { get; set; }
        public int NotesCount { get; set; }
        public int AttachmentsCount { get; set; }
        public double AverageTimeToFillDays { get; set; }
        public List<JobStatusSummaryItemEntity> JobStatusDistribution { get; set; } = new();
    }

    public class JobStatusSummaryItemEntity
    {
        public string Status { get; set; } = string.Empty;
        public int Count { get; set; }
    }
}
