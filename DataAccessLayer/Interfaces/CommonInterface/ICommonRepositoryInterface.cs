using DataAccess.Domain;

namespace DataAccessLayer.Interfaces.CommonInterface
{
    public interface ICommonRepositoryInterface<TEntity>
        where TEntity : EntityBase
    {
        Task<TEntity?> FindAsync(Guid id);
        Task<Guid> AddAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
    }
}
