using DataAccess.Domain;
using DataAccessLayer.Domain.Masters.User;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Domain.Masters.Department
{
    [Table("Departments")]
    public class DepartmentEntity :EntityBase
    {
        [Column("OwnerId")]
        public Guid? OwnerId { get; set; }

        [Column("Name")]
        public string DeptName { get; set; }

        [Column("Location")]
        public string Location { get; set; }

        [NotMapped]
        public int? JobCount { get; set; }

        [Column("Description")]
        public string? Description { get; set; }

        [Column("CreatedOn")]
        public DateTime CreatedOn { get; set; }

        [Column("CreatedBy")]
        public Guid? CreatedBy { get; set; }

        [Column("ModifiedOn")]
        public DateTime? ModifiedOn { get; set; }

        [Column("ModifiedBy")]
        public Guid? ModifiedBy { get; set; }

        public UserEntity? OwnerUser { get; set; }
        public virtual ICollection<DepartmentMembersEntity>? DepartmentMembers { get; set; } = new HashSet<DepartmentMembersEntity>();
    }
}
