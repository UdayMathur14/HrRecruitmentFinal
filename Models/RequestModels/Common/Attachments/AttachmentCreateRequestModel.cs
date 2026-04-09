using Microsoft.AspNetCore.Http;
using Models.Enums;

namespace Models.RequestModels.Common.Attachments
{
    public class AttachmentCreateRequestModel
    {
        public ReferenceType ReferenceType { get; set; }
        public Guid ReferenceId { get; set; }
        public IFormFile File { get; set; }
        public Guid? CreatedBy { get; set; }
    }
}
