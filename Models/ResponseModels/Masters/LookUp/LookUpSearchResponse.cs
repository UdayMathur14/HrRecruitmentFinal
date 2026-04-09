using Models.ResponseModels.BaseResponseSetup;

namespace Models.ResponseModels.Masters.LookUp
{ 
    public class LookUpSearchResponse : SearchResponseBase<LookUpReadResponseModel>
    {
        public List<LookUpReadResponseModel> LookUps => Results;
    }
}
