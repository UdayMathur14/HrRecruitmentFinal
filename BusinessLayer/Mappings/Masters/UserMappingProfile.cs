using AutoMapper;
using DataAccessLayer.Domain.Masters.User;
using Models.RequestModels.Masters.User;
using Models.ResponseModels.Masters.User;

namespace BusinessLayer.Mappings.Masters
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<UserEntity, UserReadResponseModel>().ReverseMap();
            CreateMap<UserCreateRequestModel, UserEntity>().ReverseMap();
            CreateMap<UserSearchResponseEntity, UserSearchResponseModel>().ReverseMap();
        }
    }
}
