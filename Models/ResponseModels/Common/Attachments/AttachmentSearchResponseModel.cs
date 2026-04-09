using Models.ResponseModels.BaseResponseSetup;

namespace Models.ResponseModels.Common.Attachments
{
    public class AttachmentSearchResponseModel : SearchResponseBase<AttachmentReadResponseModel>
    {
        public List<AttachmentReadResponseModel> Attachments => Results;
    }
}
