using DataAccess.Domain;
using DataAccessLayer.Domain.Masters.User;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Domain.Masters.Department
{
    [Table("DeptMembers")]
    public class DepartmentMembersEntity : EntityBase
    {
        [Column("DeptId")]
        public Guid DeptId { get; set; }

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

        public DepartmentEntity? Department { get; set; }
        public UserEntity? User { get; set; }
    }
}
