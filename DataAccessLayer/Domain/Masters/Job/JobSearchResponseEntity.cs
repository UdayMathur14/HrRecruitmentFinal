using Models;
using Models.ResponseModels.BaseResponseSetup;

namespace DataAccessLayer.Domain.Masters.Job
{
    public class JobSearchResponseEntity : CommonResponseModel
    {
        public IEnumerable<JobEntity>? Jobs { get; set; } = new List<JobEntity>();
        public PagingModel Paging { get; set; } = new PagingModel();
        public Dictionary<string, List<string>> Filters { get; set; } = new Dictionary<string, List<string>>();
    }
}
