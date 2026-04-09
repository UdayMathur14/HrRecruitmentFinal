using Models.Enums;

namespace Models.ResponseModels.Common.Attachments
{
    public class AttachmentReadResponseModel
    {
        public Guid Id { get; set; }
        public ReferenceType ReferenceType { get; set; }
        public Guid ReferenceId { get; set; }
        public string FilePath { get; set; }
        public string? FileName { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? ModifiedBy { get; set; }
        public string? Status { get; set; }
    }
}
