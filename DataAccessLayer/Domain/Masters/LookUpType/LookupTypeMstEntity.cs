using DataAccess.Domain.Masters.LookUpMst;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Domain.Masters.LookUpType
{
    [Table("HRMS_LookUpTypeMaster")]
    public class LookupTypeMstEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid(); 

        [Required]
        [StringLength(100)]
        public string Type { get; set; } = string.Empty;

        public string? Description { get; set; } 

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        [StringLength(100)]
        public string? CreatedBy { get; set; } 

        public DateTime ModifiedOn { get; set; } 

        [StringLength(100)]
        public string? ModifiedBy { get; set; } 

        [StringLength(100)]
        public string? Status { get; set; } 
        public ICollection<LookupMstEntity> Lookups { get; set; } = new List<LookupMstEntity>();
    }
}
