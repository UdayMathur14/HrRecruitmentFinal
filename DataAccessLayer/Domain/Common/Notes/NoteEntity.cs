using DataAccess.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Domain.Common.Notes
{
    [Table("Notes")]
    public class NoteEntity : EntityBase
    {
        [Column("ReferenceType")]
        public int ReferenceType { get; set; }

        [Column("ReferenceId")]
        public Guid ReferenceId { get; set; }

        [Column("Header")]
        public string Header { get; set; }

        [Column("Description")]
        public string? Description { get; set; }

        [Column("CreatedOn")]
        public DateTime? CreatedOn { get; set; }

        [Column("CreatedBy")]
        public Guid? CreatedBy { get; set; }

        [Column("ModifiedOn")]
        public DateTime? ModifiedOn { get; set; }

        [Column("ModifiedBy")]
        public Guid? ModifiedBy { get; set; }
    }
}
