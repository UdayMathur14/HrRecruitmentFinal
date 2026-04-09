namespace Models.RequestModels.Masters.Candidate
{
    public class CandidateSearchRequestModel
    {
        public Guid? JobId { get; set; }
        public Guid? DeptId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Status { get; set; }
        public string? CandidateStatus { get; set; }
        public string? Location { get; set; }
        public string? Skills { get; set; }
    }
}
