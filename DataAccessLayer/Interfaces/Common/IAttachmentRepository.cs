using DataAccessLayer.Domain.Common.Attachments;
using DataAccessLayer.Interfaces.CommonInterface;
using Models.RequestModels.Common.Attachments;

namespace DataAccessLayer.Interfaces.Common
{
    public interface IAttachmentRepository : ICommonRepositoryInterface<AttachmentEntity>
    {
        Task<AttachmentSearchResponseEntity> SearchAttachmentAsync(AttachmentSearchRequestModel requestModel, string? offset, string count);
    }
}
