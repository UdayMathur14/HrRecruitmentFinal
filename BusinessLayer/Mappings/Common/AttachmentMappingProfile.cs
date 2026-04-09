using AutoMapper;
using DataAccessLayer.Domain.Common.Attachments;
using Models.Enums;
using Models.RequestModels.Common.Attachments;
using Models.ResponseModels.Common.Attachments;

namespace BusinessLayer.Mappings.Common
{
    public class AttachmentMappingProfile : Profile
    {
        public AttachmentMappingProfile()
        {
            CreateMap<AttachmentEntity, AttachmentReadResponseModel>()
                .ForMember(dest => dest.ReferenceType, opt => opt.MapFrom(src => (ReferenceType)src.ReferenceType));

            CreateMap<AttachmentReadResponseModel, AttachmentEntity>()
                .ForMember(dest => dest.ReferenceType, opt => opt.MapFrom(src => (int)src.ReferenceType));

            CreateMap<AttachmentCreateRequestModel, AttachmentEntity>();
            CreateMap<AttachmentSearchResponseEntity, AttachmentSearchResponseModel>().ReverseMap();
        }
    }
}
