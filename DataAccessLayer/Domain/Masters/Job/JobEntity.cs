using DataAccess.Domain;
using DataAccessLayer.Domain.Masters.User;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Domain.Masters.Job
{
    [Table("Jobs")]
    public class JobEntity : EntityBase
    {
        [Column("DeptId")]
        public Guid DeptId { get; set; }

        [Column("JobName")]
        public string JobName { get; set; }

        [Column("Description")]
        public string? Description { get; set; }

        [Column("HeadCount")]
        public int HeadCount { get; set; }

        [Column("JobStage")]
        public string? JobStage { get; set; }

        [Column("JobOwnerId")]
        public Guid? JobOwnerId { get; set; }

        [Column("CreatedOn")]
        public DateTime CreatedOn { get; set; }

        [Column("CreatedBy")]
        public Guid? CreatedBy { get; set; }

        [Column("ModifiedOn")]
        public DateTime? ModifiedOn { get; set; }

        [Column("ModifiedBy")]
        public Guid? ModifiedBy { get; set; }

        public UserEntity? JobOwnerUser { get; set; }
        public virtual ICollection<JobMembersEntity>? JobMembers { get; set; } = new HashSet<JobMembersEntity>();
    }
}
