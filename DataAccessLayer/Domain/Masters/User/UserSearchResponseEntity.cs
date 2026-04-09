using Models;
using Models.ResponseModels.BaseResponseSetup;

namespace DataAccessLayer.Domain.Masters.User
{
    public class UserSearchResponseEntity : CommonResponseModel
    {
        public IEnumerable<UserEntity>? Users { get; set; } = new List<UserEntity>();
        public PagingModel Paging { get; set; } = new PagingModel();
        public Dictionary<string, List<string>> Filters { get; set; } = new Dictionary<string, List<string>>();
    }
}
