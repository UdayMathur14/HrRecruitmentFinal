using DataAccess.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Domain.Masters.Candidate
{
    [Table("Candidates")]
    public class CandidateEntity : EntityBase
    {
        [Column("JobId")]
        public Guid JobId { get; set; }

        [Column("DeptId")]
        public Guid DeptId { get; set; }

        [Column("FullName")]
        public string FullName { get; set; }

        [Column("Email")]
        public string Email { get; set; }

        [Column("Phone")]
        public string? Phone { get; set; }

        [Column("Gender")]
        public string? Gender { get; set; }

        [Column("Education")]
        public string? Education { get; set; }

        [Column("University")]
        public string? University { get; set; }

        [Column("CurrentTitle")]
        public string? CurrentTitle { get; set; }

        [Column("CurrentCompany")]
        public string? CurrentCompany { get; set; }

        [Column("ExperienceYears")]
        public decimal? ExperienceYears { get; set; }

        [Column("Location")]
        public string? Location { get; set; }

        [Column("Skills")]
        public string? Skills { get; set; }

        [Column("CurrentSalary")]
        public decimal? CurrentSalary { get; set; }

        [Column("ExpectedSalary")]
        public decimal? ExpectedSalary { get; set; }

        [Column("LinkedInProfile")]
        public string? LinkedInProfile { get; set; }

        [Column("CVPath")]
        public string? CVPath { get; set; }
        [Column("CandidateStatus")]
        public string? CandidateStatus { get; set; }

        [Column("AIScore")]
        public int? AIScore { get; set; }

        [Column("Summary")]
        public string? Summary { get; set; }

        [Column("CreatedOn")]
        public DateTime CreatedOn { get; set; }

        [Column("CreatedBy")]
        public Guid? CreatedBy { get; set; }

        [Column("ModifiedOn")]
        public DateTime? ModifiedOn { get; set; }

        [Column("ModifiedBy")]
        public Guid? ModifiedBy { get; set; }
    }
}
