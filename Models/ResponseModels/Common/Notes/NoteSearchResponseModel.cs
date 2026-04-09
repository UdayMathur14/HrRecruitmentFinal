using Models.ResponseModels.BaseResponseSetup;

namespace Models.ResponseModels.Common.Notes
{
    public class NoteSearchResponseModel : SearchResponseBase<NoteReadResponseModel>
    {
        public List<NoteReadResponseModel> Notes => Results;
    }
}
