using Models.ResponseModels.BaseResponseSetup;

namespace Models.ResponseModels.Masters.User
{
    public class UserSearchResponseModel : SearchResponseBase<UserReadResponseModel>
    {
        public List<UserReadResponseModel> Users => Results;
    }
}
