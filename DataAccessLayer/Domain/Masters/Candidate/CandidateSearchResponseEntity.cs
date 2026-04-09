using Models;
using Models.ResponseModels.BaseResponseSetup;

namespace DataAccessLayer.Domain.Masters.Candidate
{
    public class CandidateSearchResponseEntity : CommonResponseModel
    {
        public IEnumerable<CandidateEntity>? Candidates { get; set; } = new List<CandidateEntity>();
        public PagingModel Paging { get; set; } = new PagingModel();
        public Dictionary<string, List<string>> Filters { get; set; } = new Dictionary<string, List<string>>();
    }
}
