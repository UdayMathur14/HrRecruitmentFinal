using Models;
using Models.RequestModels.Masters.User;
using Models.ResponseModels.Masters.User;

namespace BusinessLayer.Interfaces.Masters
{
    public interface IUserService
    {
        Task<UserReadResponseModel?> GetByIdAsync(Guid id);
        Task<CommonResponseModel> CreateUserAsync(UserCreateRequestModel requestModel);
        Task<UserSearchResponseModel?> SearchUserAsync(UserSearchRequestModel requestModel, string? offset, string count);
        Task<CommonResponseModel> UpdateUserAsync(Guid id, UserUpdateRequestModel requestModel);
    }
}
