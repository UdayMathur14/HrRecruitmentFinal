using DataAccessLayer.Domain.Common.Notes;
using DataAccessLayer.Interfaces.CommonInterface;
using Models.RequestModels.Common.Notes;

namespace DataAccessLayer.Interfaces.Common
{
    public interface INoteRepository : ICommonRepositoryInterface<NoteEntity>
    {
        Task<NoteSearchResponseEntity> SearchNoteAsync(NoteSearchRequestModel requestModel, string? offset, string count);
    }
}
