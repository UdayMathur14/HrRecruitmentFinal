using DataAccessLayer.Interfaces.Common;
using Microsoft.EntityFrameworkCore;
using Models.Enums;

namespace DataAccessLayer.Repositories.Common
{
    public class ReferenceValidationRepository(ApplicationDbContext context) : IReferenceValidationRepository
    {
        public async Task<bool> IsReferenceValidAsync(ReferenceType referenceType, Guid referenceId)
        {
            return referenceType switch
            {
                ReferenceType.Department => await context.DepartmentEntity
                    .AnyAsync(x => x.Id == referenceId),

                ReferenceType.Job => await context.JobEntity
                    .AnyAsync(x => x.Id == referenceId),

                ReferenceType.Candidate => await context.CandidateEntity
                    .AnyAsync(x => x.Id == referenceId),

                _ => false
            };
        }
    }
}
