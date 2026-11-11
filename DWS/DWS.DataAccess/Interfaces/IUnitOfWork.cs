
using System.Threading;
using System.Threading.Tasks;

namespace DWS.DataAccess.Interfaces;

// Defines the contract for managing repositories and database operations.
public interface IUnitOfWork
{
    // Provides access to a generic repository for any entity type.
    IGenericRepository<TEntity> Repository<TEntity>()
        where TEntity : class;

    // Saves all pending changes to the database.
    Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default);
}
