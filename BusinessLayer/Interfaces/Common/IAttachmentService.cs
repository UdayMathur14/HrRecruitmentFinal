using Models;
using Models.RequestModels.Common.Attachments;
using Models.ResponseModels.Common.Attachments;

namespace BusinessLayer.Interfaces.Common
{
    public interface IAttachmentService
    {
        Task<AttachmentReadResponseModel?> GetByIdAsync(Guid id);
        Task<CommonResponseModel> CreateAttachmentAsync(AttachmentCreateRequestModel requestModel);
        Task<AttachmentSearchResponseModel?> SearchAttachmentAsync(AttachmentSearchRequestModel requestModel, string? offset, string count);
        Task<CommonResponseModel> UpdateAttachmentAsync(Guid id, AttachmentUpdateRequestModel requestModel);
    }
}
