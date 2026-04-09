using DataAccessLayer.Domain.Masters.User;
using DataAccessLayer.Interfaces.CommonInterface;
using Models.RequestModels.Masters.User;

namespace DataAccessLayer.Interfaces.Masters
{
    public interface IUserRepository : ICommonRepositoryInterface<UserEntity>
    {
        Task<UserSearchResponseEntity> SearchUserAsync(UserSearchRequestModel requestModel, string? offset, string count);
    }
}
