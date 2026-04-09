using Models.ResponseModels.BaseResponseSetup;

namespace Models.ResponseModels.Masters.LookUpType
{
    public class LookUpTypeSearchResponse : SearchResponseBase<LookUpTypeReadResponseModel>
    {
        public List<LookUpTypeReadResponseModel> LookUpTypes => base.Results;
    }
}
