using Models;
using Models.ResponseModels.BaseResponseSetup;

namespace DataAccessLayer.Domain.Common.Notes
{
    public class NoteSearchResponseEntity : CommonResponseModel
    {
        public IEnumerable<NoteEntity>? Notes { get; set; } = new List<NoteEntity>();
        public PagingModel Paging { get; set; } = new PagingModel();
        public Dictionary<string, List<string>> Filters { get; set; } = new Dictionary<string, List<string>>();
    }
}
