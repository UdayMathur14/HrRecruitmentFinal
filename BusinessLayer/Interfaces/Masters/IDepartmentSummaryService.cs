using Models.RequestModels.Masters.DepartmentSummary;
using Models.ResponseModels.Masters.DepartmentSummary;

namespace BusinessLayer.Interfaces.Masters
{
    public interface IDepartmentSummaryService
    {
        Task<DepartmentSummaryResponseModel> GetSummaryAsync(DepartmentSummaryRequestModel requestModel);
    }
}
