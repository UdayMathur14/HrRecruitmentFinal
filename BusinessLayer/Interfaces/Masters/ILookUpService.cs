using Models;
using Models.RequestModels.Masters.LookUp;
using Models.ResponseModels.Masters.LookUp;

namespace BusinessLogic.Interfaces.Masters
{
    public interface ILookUpService
    {

        Task<LookUpCreateResponseModel?> CreateLookUpAsync(LookUpRequestModel lookUp);
        Task<LookUpReadResponseModel> GetLookUpAsync(Guid id);
        Task<LookUpSearchResponse> SearchLookUpAsync(LookUpSearchRequestModel requestModel, string loginUserId, string? offset, string v);
        Task<CommonResponseModel> UpdateLookUpAsync(LookUpRequestModel lookUp, Guid id);
    }
}
