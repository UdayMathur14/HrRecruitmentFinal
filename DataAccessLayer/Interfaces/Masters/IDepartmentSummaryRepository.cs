using DataAccessLayer.Domain.Masters.DepartmentSummary;

namespace DataAccessLayer.Interfaces.Masters
{
    public interface IDepartmentSummaryRepository
    {
        Task<DepartmentSummaryResponseEntity?> GetSummaryAsync(Guid? deptId, Guid? jobId);
    }
}
