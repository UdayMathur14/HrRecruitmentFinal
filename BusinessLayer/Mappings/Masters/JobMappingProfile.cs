using AutoMapper;
using DataAccessLayer.Domain.Masters.Job;
using DataAccessLayer.Domain.Masters.User;
using Models.RequestModels.Masters.Job;
using Models.ResponseModels.Masters.Job;

namespace BusinessLayer.Mappings.Masters
{
    public class JobMappingProfile : Profile
    {
        public JobMappingProfile()
        {
            CreateMap<JobEntity, JobReadResponseModel>()
                .ForMember(dest => dest.Owner, opt => opt.MapFrom(src => src.JobOwnerUser));
            CreateMap<JobCreateRequestModel, JobEntity>()
                .ForMember(dest => dest.JobMembers, opt => opt.MapFrom(src => src.JobMembers));
            CreateMap<JobMemberRequestModel, JobMembersEntity>();
            CreateMap<UserEntity, JobMemberReadResponseModel>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}".Trim()));
            CreateMap<JobMembersEntity, JobMemberReadResponseModel>()
                .ForMember(dest => dest.UserName,
                    opt => opt.MapFrom(src => src.User != null ? $"{src.User.FirstName} {src.User.LastName}".Trim() : null));
            CreateMap<JobSearchResponseEntity, JobSearchResponseModel>();
        }
    }
}
