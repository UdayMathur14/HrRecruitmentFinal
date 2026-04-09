using Models;
using Models.RequestModels.Masters.Candidate;
using Models.ResponseModels.Masters.Candidate;

namespace BusinessLayer.Interfaces.Masters
{
    public interface ICandidateService
    {
        Task<CandidateReadResponseModel?> GetByIdAsync(Guid id);
        Task<CommonResponseModel> CreateCandidateAsync(CandidateCreateRequestModel requestModel);
        Task<CandidateSearchResponseModel?> SearchCandidateAsync(CandidateSearchRequestModel requestModel, string? offset, string count);
        Task<CommonResponseModel> UpdateCandidateAsync(Guid id, CandidateUpdateRequestModel requestModel);
    }
}
