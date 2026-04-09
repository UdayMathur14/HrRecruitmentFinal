

using Models;
using Models.RequestModels.Masters.LookUpType;
using Models.ResponseModels.Masters.LookUpType;

namespace BusinessLogic.Interfaces.Masters
{
    public interface ILookUpTypeService
    {
        Task<LookUpTypeCreateResponseModel?> CreateLookUpAsync(LookUpTypeRequestModel lookUp);
        Task<LookUpTypeReadResponseModel> GetLookUpTypeAsync(Guid id);
        Task<LookUpTypeSearchResponse?> SearchLookUpAsync(LookUpTypeSearchRequestModel requestModel, string? offset, string count);
        Task<CommonResponseModel?> UpdateLookUpTypeAsync(LookUpTypeRequestModel lookUp, Guid id);
    }
}
