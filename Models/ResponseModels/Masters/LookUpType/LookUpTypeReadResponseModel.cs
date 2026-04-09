
using System.ComponentModel.DataAnnotations;

namespace Models.ResponseModels.Masters.LookUpType
{
    public class LookUpTypeReadResponseModel
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public string? Status { get; set; }
    }
}
