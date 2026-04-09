using DataAccessLayer.Domain.Masters.DepartmentSummary;
using DataAccessLayer.Interfaces.Masters;
using Microsoft.EntityFrameworkCore;
using Models.Enums;

namespace DataAccessLayer.Repositories.Masters
{
    public class DepartmentSummaryRepository(ApplicationDbContext context) : IDepartmentSummaryRepository
    {
        public async Task<DepartmentSummaryResponseEntity?> GetSummaryAsync(Guid? deptId, Guid? jobId)
        {
            Guid resolvedDepartmentId;
            Guid? resolvedJobId = jobId;

            if (jobId.HasValue)
            {
                var jobContext = await context.JobEntity
                    .AsNoTracking()
                    .Where(x => x.Id == jobId.Value)
                    .Select(x => new { x.Id, x.DeptId })
                    .FirstOrDefaultAsync();

                if (jobContext == null)
                    return null;

                resolvedDepartmentId = jobContext.DeptId;

                if (deptId.HasValue && deptId.Value != resolvedDepartmentId)
                    return null;
            }
            else if (deptId.HasValue)
            {
                resolvedDepartmentId = deptId.Value;
            }
            else
            {
                return null;
            }

            var department = await context.DepartmentEntity
                .AsNoTracking()
                .Where(x => x.Id == resolvedDepartmentId)
                .Select(x => new
                {
                    x.Id,
                    x.DeptName,
                    x.Description,
                    TeamMembers = x.DepartmentMembers.Count()
                })
                .FirstOrDefaultAsync();

            if (department == null)
                return null;

            var jobsQuery = context.JobEntity
                .AsNoTracking()
                .Where(x => x.DeptId == resolvedDepartmentId);

            var totalJobs = await jobsQuery.CountAsync();
            var activePositions = await jobsQuery.CountAsync(x =>
                x.Status != null && x.Status.ToLower() == "active");

            var averageTimeToFillDays = await jobsQuery
                .Where(x => x.ModifiedOn.HasValue && x.Status != null && x.Status.ToLower() == "completed")
                .AverageAsync(x => (double?)EF.Functions.DateDiffDay(x.CreatedOn, x.ModifiedOn!.Value)) ?? 0;

            var notesCount = await context.NoteEntity
                .AsNoTracking()
                .CountAsync(x => x.ReferenceType == (int)ReferenceType.Department && x.ReferenceId == resolvedDepartmentId);

            var attachmentsCount = await context.AttachmentEntity
                .AsNoTracking()
                .CountAsync(x => x.ReferenceType == (int)ReferenceType.Department && x.ReferenceId == resolvedDepartmentId);

            var jobStatusDistribution = await jobsQuery
                .GroupBy(x => x.Status)
                .Select(group => new JobStatusSummaryItemEntity
                {
                    Status = string.IsNullOrWhiteSpace(group.Key) ? "Unknown" : group.Key!,
                    Count = group.Count()
                })
                .OrderByDescending(x => x.Count)
                .ThenBy(x => x.Status)
                .ToListAsync();

            return new DepartmentSummaryResponseEntity
            {
                DepartmentId = department.Id,
                JobId = resolvedJobId,
                DepartmentName = department.DeptName,
                DepartmentDescription = department.Description,
                TotalJobs = totalJobs,
                ActivePositions = activePositions,
                TeamMembers = department.TeamMembers,
                NotesCount = notesCount,
                AttachmentsCount = attachmentsCount,
                AverageTimeToFillDays = Math.Round(averageTimeToFillDays, 2),
                JobStatusDistribution = jobStatusDistribution
            };
        }
    }
}
