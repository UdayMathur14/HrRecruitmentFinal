using Models;
using Models.RequestModels.Common.Notes;
using Models.ResponseModels.Common.Notes;

namespace BusinessLayer.Interfaces.Common
{
    public interface INoteService
    {
        Task<NoteReadResponseModel?> GetByIdAsync(Guid id);
        Task<CommonResponseModel> CreateNoteAsync(NoteCreateRequestModel requestModel);
        Task<NoteSearchResponseModel?> SearchNoteAsync(NoteSearchRequestModel requestModel, string? offset, string count);
        Task<CommonResponseModel> UpdateNoteAsync(Guid id, NoteUpdateRequestModel requestModel);
    }
}
