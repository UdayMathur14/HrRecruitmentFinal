using AutoMapper;
using DataAccessLayer.Domain.Common.Notes;
using Models.RequestModels.Common.Notes;
using Models.ResponseModels.Common.Notes;

namespace BusinessLayer.Mappings.Common
{
    public class NoteMappingProfile : Profile
    {
        public NoteMappingProfile()
        {
            CreateMap<NoteEntity, NoteReadResponseModel>().ReverseMap();
            CreateMap<NoteCreateRequestModel, NoteEntity>();
            CreateMap<NoteSearchResponseEntity, NoteSearchResponseModel>().ReverseMap();
        }
    }
}
