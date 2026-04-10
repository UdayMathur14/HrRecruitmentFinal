using AutoMapper;
using DataAccessLayer.Domain.Masters.Department;
using DataAccessLayer.Domain.Masters.Job;
using DataAccessLayer.Domain.Masters.User;
using Models.RequestModels.Masters.Department;
using Models.ResponseModels.Masters.Department;
using Models.ResponseModels.Masters.Job;

namespace BusinessLayer.Mappings.Masters
{
    public class DepartmentMappingProfile : Profile
    {
        public DepartmentMappingProfile() { 
        
            CreateMap<DepartmentEntity, DepartmentReadResponseModel>()
                .ForMember(dest => dest.Owner,
                    opt => opt.MapFrom(src => src.OwnerUser));

            CreateMap<DepartmentCreateRequestModel, DepartmentEntity>()
            .ForMember(dest => dest.DepartmentMembers, opt => opt.MapFrom(src => src.DepartmentMembers));

            CreateMap<DeptMemberRequestModel, DepartmentMembersEntity>();
            CreateMap<UserEntity, DeptMemberReadResponseModel>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}".Trim()));
            CreateMap<DepartmentMembersEntity, DeptMemberReadResponseModel>()
                .ForMember(dest => dest.UserName,
                    opt => opt.MapFrom(src => src.User != null ? $"{src.User.FirstName} {src.User.LastName}".Trim() : null));

            CreateMap<DepartmentSearchResponseEntity , DeptSearchResponseModel>();

            CreateMap<JobEntity , JobReadResponseModel>();

        }
    }
}
