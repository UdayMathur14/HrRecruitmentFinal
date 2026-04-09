using Models.ResponseModels.BaseResponseSetup;

namespace Models.ResponseModels.Masters.Candidate
{
    public class CandidateSearchResponseModel : SearchResponseBase<CandidateReadResponseModel>
    {
        public List<CandidateReadResponseModel> Candidates => Results;
    }
}
