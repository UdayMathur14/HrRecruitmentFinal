using DataAccess.Domain;
using DataAccessLayer.Domain.Masters.User;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Domain.Masters.Job
{
    [Table("JobMembers")]
    public class JobMembersEntity : EntityBase
    {
        [Column("JobId")]
        public Guid JobId { get; set; }

        [Column("UserId")]
        public Guid UserId { get; set; }

        [Column("CreatedOn")]
        public DateTime? CreatedOn { get; set; }

        [Column("CreatedBy")]
        public Guid? CreatedBy { get; set; }

        [Column("ModifiedOn")]
        public DateTime? ModifiedOn { get; set; }

        [Column("ModifiedBy")]
        public Guid? ModifiedBy { get; set; }

        public JobEntity? Job { get; set; }
        public UserEntity? User { get; set; }
    }
}
