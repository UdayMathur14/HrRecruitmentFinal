using DataAccess.Domain.Masters.LookUpType;
using Models.RequestModels.Masters.LookUpType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces.Masters
{
    public interface ILookupTypeReporsitory
    {
        Task<Guid> AddAsync(LookupTypeMstEntity lookup);
        Task<LookupTypeMstEntity?> FindById(Guid id);
        Task<LookupTypeMstEntity?> FindByTypeAsync(LookUpTypeRequestModel requestModel);
        Task<LookupTypeSearchResponseEntity> SearLookupTypeAsync(LookUpTypeSearchRequestModel requestModel, string? offset, string count);
        Task<Guid> UpdateAsync(LookupTypeMstEntity entity);
    }
}
