using DataAccessLayer.Domain.Masters.Candidate;
using DataAccessLayer.Interfaces.CommonInterface;
using Models.RequestModels.Masters.Candidate;

namespace DataAccessLayer.Interfaces.Masters
{
    public interface ICandidateRepository : ICommonRepositoryInterface<CandidateEntity>
    {
        Task<CandidateSearchResponseEntity> SearchCandidateAsync(CandidateSearchRequestModel requestModel, string? offset, string count);
    }
}
