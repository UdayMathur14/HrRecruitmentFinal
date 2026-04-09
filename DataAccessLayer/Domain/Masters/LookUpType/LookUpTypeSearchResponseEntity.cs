using Models;
using Models.ResponseModels.BaseResponseSetup;

namespace DataAccess.Domain.Masters.LookUpType
{
    public class LookupTypeSearchResponseEntity: CommonResponseModel
    {
        public IEnumerable<LookupTypeMstEntity>? LookUpTypes { get; set; } = new List<LookupTypeMstEntity>();
        public PagingModel Paging { get; set; } = new PagingModel();
        public Dictionary<string, List<string>> Filters { get; set; } = new Dictionary<string, List<string>>();
    }
}
