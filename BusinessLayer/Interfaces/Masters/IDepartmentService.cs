using Models;
using Models.RequestModels.Masters.Department;
using Models.ResponseModels.Masters.Department;

namespace BusinessLayer.Interfaces.Masters
{
    public interface IDepartmentService
    {
        Task<List<DepartmentReadResponseModel>> GetAllAsync();

        Task<DepartmentReadResponseModel?> GetByIdAsync(Guid id);

        Task<CommonResponseModel> CreateFullDepartmentAsync(DepartmentCreateRequestModel DeptModel);

        Task<DeptSearchResponseModel?> SearchDeptAsync(DepartmentSearchRequestModel requestModel, string? offset, string count);

        Task<CommonResponseModel> UpdateDepartmentAsync(Guid id, DepartmentUpdateRequestModel requestModel);
    }
}
