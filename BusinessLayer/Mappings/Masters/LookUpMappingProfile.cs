using AutoMapper;
using DataAccess.Domain.Masters.LookUpMst;
using Models.RequestModels.Masters.LookUp;
using Models.ResponseModels.Masters.LookUp;

namespace BusinessLogic.Mappings.Masters
{
    public class LookUpMappingProfile : Profile
    {
        public LookUpMappingProfile()
        {
            CreateMap<LookUpRequestModel, LookupMstEntity>();
            CreateMap<LookupSearchResponseEntity, LookUpSearchResponse>();
            CreateMap<LookupMstEntity, LookUpReadResponseModel>();
        }
    }
}
