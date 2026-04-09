using DataAccess.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Domain.Common.Attachments
{
    [Table("Attachments")]
    public class AttachmentEntity : EntityBase
    {
        [Column("ReferenceType")]
        public int ReferenceType { get; set; }

        [Column("ReferenceId")]
        public Guid ReferenceId { get; set; }

        [Column("FilePath")]
        public string FilePath { get; set; }

        [Column("FileName")]
        public string? FileName { get; set; }

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
