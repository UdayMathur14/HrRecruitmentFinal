using Models;
using Models.ResponseModels.BaseResponseSetup;

namespace DataAccess.Domain.Masters.LookUpMst
{
    public class LookupSearchResponseEntity : CommonResponseModel
    {
        public IEnumerable<LookupMstEntity>? LookUps { get; set; } = new List<LookupMstEntity>();
        public PagingModel Paging { get; set; } = new PagingModel();
        public Dictionary<string, List<string>> Filters { get; set; } = new Dictionary<string, List<string>>();

    }
}
