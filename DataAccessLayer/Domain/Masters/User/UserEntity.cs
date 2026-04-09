using DataAccess.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Domain.Masters.User
{
    [Table("Users")]
    public class UserEntity : EntityBase
    {
        [Column("FirstName")]
        public string FirstName { get; set; }

        [Column("LastName")]
        public string LastName { get; set; }

        [Column("Email")]
        public string Email { get; set; }

        [Column("PhoneNumber")]
        public string? PhoneNumber { get; set; }

        [Column("PasswordHash")]
        public string PasswordHash { get; set; }

        [Column("Role")]
        public string? Role { get; set; }

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
