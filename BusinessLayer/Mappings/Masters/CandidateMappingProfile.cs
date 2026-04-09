using AutoMapper;
using DataAccessLayer.Domain.Masters.Candidate;
using Models.RequestModels.Masters.Candidate;
using Models.ResponseModels.Masters.Candidate;

namespace BusinessLayer.Mappings.Masters
{
    public class CandidateMappingProfile : Profile
    {
        public CandidateMappingProfile()
        {
            CreateMap<CandidateEntity, CandidateReadResponseModel>();
            CreateMap<CandidateCreateRequestModel, CandidateEntity>();
            CreateMap<CandidateSearchResponseEntity, CandidateSearchResponseModel>()
                .ForMember(dest => dest.Results, opt => opt.MapFrom(src => src.Candidates));
        }
    }
}
