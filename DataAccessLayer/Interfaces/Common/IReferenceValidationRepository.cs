using Models.Enums;

namespace DataAccessLayer.Interfaces.Common
{
    public interface IReferenceValidationRepository
    {
        Task<bool> IsReferenceValidAsync(ReferenceType referenceType, Guid referenceId);
    }
}
