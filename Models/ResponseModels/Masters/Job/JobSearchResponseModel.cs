using Models.ResponseModels.BaseResponseSetup;

namespace Models.ResponseModels.Masters.Job
{
    public class JobSearchResponseModel : SearchResponseBase<JobReadResponseModel>
    {
        public List<JobReadResponseModel> Jobs => Results;
    }
}
