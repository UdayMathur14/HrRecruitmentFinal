using DataAccessLayer.Domain.Masters.Job;
using DataAccessLayer.Interfaces.CommonInterface;
using Models.RequestModels.Masters.Job;

namespace DataAccessLayer.Interfaces.Masters
{
    public interface IJobRepository : ICommonRepositoryInterface<JobEntity>
    {
        Task<JobSearchResponseEntity> SearchJobAsync(JobSearchRequestModel requestModel, string? offset, string count);
        Task ReplaceMembersAsync(Guid jobId, List<JobMembersEntity> members);
    }
}
