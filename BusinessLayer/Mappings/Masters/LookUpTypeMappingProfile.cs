using AutoMapper;
using DataAccess.Domain.Masters.LookUpType;
using Models.RequestModels.Masters.LookUpType;
using Models.ResponseModels.Masters.LookUpType;

namespace BusinessLogic.Mappings.Masters
{
    public class LookUpTypeMappingProfile : Profile
    {
        public LookUpTypeMappingProfile()
        {
            CreateMap<LookUpTypeRequestModel, LookupTypeMstEntity>();
            CreateMap<LookupTypeSearchResponseEntity, LookUpTypeSearchResponse>();
            CreateMap<LookupTypeMstEntity, LookUpTypeReadResponseModel>();
        }
    }
}
