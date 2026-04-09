using DataAccess.Domain.Masters.LookUpType;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Domain.Masters.LookUpMst
{
    [Table("HRMS_LookUpMaster")]
    public class LookupMstEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid(); 
        [Required]
        public Guid TypeId { get; set; } 

        [StringLength(100)]
        public string? Type { get; set; } 

        [StringLength(100)]
        public string? Value { get; set; } 

        [StringLength(200)]
        public string? Description { get; set; } 

        [StringLength(200)]
        public string? URL { get; set; } 

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow; 

        [StringLength(100)]
        public string? CreatedBy { get; set; } 

        public DateTime ModifiedOn { get; set; } 

        [StringLength(100)]
        public string? ModifiedBy { get; set; }

        [StringLength(100)]
        public string? Status { get; set; } 
        public virtual LookupTypeMstEntity? LookupTypeDetails { get; set; }

    }
}
