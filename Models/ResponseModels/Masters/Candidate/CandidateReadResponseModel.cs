namespace Models.ResponseModels.Masters.Candidate
{
    public class CandidateReadResponseModel
    {
        public Guid Id { get; set; }
        public Guid JobId { get; set; }
        public Guid DeptId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string? Gender { get; set; }
        public string? Education { get; set; }
        public string? University { get; set; }
        public string? CurrentTitle { get; set; }
        public string? CurrentCompany { get; set; }
        public string? Summary { get; set; }
        public decimal? ExperienceYears { get; set; }
        public string? Location { get; set; }
        public string? Skills { get; set; }
        public decimal? CurrentSalary { get; set; }
        public decimal? ExpectedSalary { get; set; }
        public string? LinkedInProfile { get; set; }
        public string? CVPath { get; set; }
        public int? AIScore { get; set; }
        public string? Status { get; set; }
        public string? CandidateStatus { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? ModifiedBy { get; set; }
    }
}
