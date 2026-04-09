using DataAccess.Domain.Masters.LookUpMst;
using Models.RequestModels.Masters.LookUp;

namespace DataAccess.Interfaces.Masters
{
    public interface ILookupReporsitory
    {
        Task<Guid> AddAsync(LookupMstEntity entity);
        Task<LookupMstEntity?> FindAsync(Guid id);
        Task<LookupMstEntity?> FindByValue(LookUpRequestModel requestModel);
        Task<LookupSearchResponseEntity> SearchLookUpAsync(LookUpSearchRequestModel requestModel, string? offset, string count);
        Task<Guid> UpdateAsync(LookupMstEntity entity);
    }
}
