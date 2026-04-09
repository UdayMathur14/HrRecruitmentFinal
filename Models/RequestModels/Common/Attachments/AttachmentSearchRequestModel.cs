using Models.Enums;

namespace Models.RequestModels.Common.Attachments
{
    public class AttachmentSearchRequestModel
    {
        public ReferenceType? ReferenceType { get; set; }
        public Guid? ReferenceId { get; set; }
        public string? FileName { get; set; }
        public string? Status { get; set; }
    }
}
