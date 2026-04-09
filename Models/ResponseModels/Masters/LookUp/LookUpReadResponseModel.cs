using Models.ResponseModels.Masters.LookUpType;
using System.ComponentModel.DataAnnotations;

namespace Models.ResponseModels.Masters.LookUp
{
    public class LookUpReadResponseModel 
    {
        public Guid Id { get; set; } 
        public Guid TypeId { get; set; } 
        public string? Type { get; set; } 
        public string? Value { get; set; } 
        public string? Description { get; set; }
        public string? URL { get; set; }
        public DateTime CreatedOn { get; set; } 
        public string? CreatedBy { get; set; } 
        public DateTime ModifiedOn { get; set; } 
        public string? ModifiedBy { get; set; } 
        public string? Status { get; set; } 
    }
}