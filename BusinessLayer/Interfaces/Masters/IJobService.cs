using Models;
using Models.RequestModels.Masters.Job;
using Models.ResponseModels.Masters.Job;

namespace BusinessLayer.Interfaces.Masters
{
    public interface IJobService
    {
        Task<JobReadResponseModel?> GetByIdAsync(Guid id);
        Task<CommonResponseModel> CreateJobAsync(JobCreateRequestModel requestModel);
        Task<JobSearchResponseModel?> SearchJobAsync(JobSearchRequestModel requestModel, string? offset, string count);
        Task<CommonResponseModel> UpdateJobAsync(Guid id, JobUpdateRequestModel requestModel);
    }
}
