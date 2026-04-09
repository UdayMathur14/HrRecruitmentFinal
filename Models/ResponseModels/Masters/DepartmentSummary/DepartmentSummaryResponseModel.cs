using Models;

namespace Models.ResponseModels.Masters.DepartmentSummary
{
    public class DepartmentSummaryResponseModel : CommonResponseModel
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
        public List<JobStatusSummaryItemResponseModel> JobStatusDistribution { get; set; } = new();
    }

    public class JobStatusSummaryItemResponseModel
    {
        public string Status { get; set; } = string.Empty;
        public int Count { get; set; }
    }
}
