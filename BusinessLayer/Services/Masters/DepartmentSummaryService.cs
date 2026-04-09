using AutoMapper;
using BusinessLayer.Interfaces.Masters;
using DataAccessLayer.Interfaces.Masters;
using Microsoft.AspNetCore.Http;
using Models.RequestModels.Masters.DepartmentSummary;
using Models.ResponseModels.Masters.DepartmentSummary;

namespace BusinessLayer.Services.Masters
{
    public class DepartmentSummaryService(
        IDepartmentSummaryRepository departmentSummaryRepository,
        IMapper mapper) : IDepartmentSummaryService
    {
        public async Task<DepartmentSummaryResponseModel> GetSummaryAsync(DepartmentSummaryRequestModel requestModel)
        {
            if (!requestModel.DeptId.HasValue && !requestModel.JobId.HasValue)
            {
                return new DepartmentSummaryResponseModel
                {
                    responseCode = StatusCodes.Status400BadRequest,
                    message = "DeptId or JobId is required."
                };
            }

            var summaryEntity = await departmentSummaryRepository.GetSummaryAsync(requestModel.DeptId, requestModel.JobId);

            if (summaryEntity == null)
            {
                return new DepartmentSummaryResponseModel
                {
                    responseCode = StatusCodes.Status400BadRequest,
                    message = "Department or Job data not found."
                };
            }

            var response = mapper.Map<DepartmentSummaryResponseModel>(summaryEntity);
            response.responseCode = StatusCodes.Status200OK;
            response.message = "Summary fetched successfully.";
            return response;
        }
    }
}
