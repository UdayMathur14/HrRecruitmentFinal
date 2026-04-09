using DataAccess.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Domain
{
    public abstract class EntityBase : IEntity
    {
        [Key]
        [Column("ID")]
        public Guid Id { get; set; }
        public string? Status { get; set; }
    }
}
