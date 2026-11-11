
using DWS.DataAccess.Context;
using DWS.DataAccess.Interfaces;

namespace DWS.DataAccess.Repositories;

// Implements the Unit of Work pattern to manage repositories and database operations.
public class UnitOfWork : IUnitOfWork
{
    // Holds the application's database context.
    private readonly ApplicationDbContext _context;

    // Stores created repositories to avoid creating duplicate instances.
    private readonly Dictionary<Type, object> _repositories = new();

    // Constructor: Receives ApplicationDbContext through dependency injection.
    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    // Returns an existing repository or creates a new one for the entity type.
    public IGenericRepository<TEntity> Repository<TEntity>()
        where TEntity : class
    {
        var entityType = typeof(TEntity);

        // Check whether a repository already exists for this entity.
        if (!_repositories.TryGetValue(entityType, out var repository))
        {
            // Create and store a new generic repository.
            repository = new GenericRepository<TEntity>(_context);
            _repositories.Add(entityType, repository);
        }

        return (IGenericRepository<TEntity>)repository;
    }

    // Saves all pending database changes.
    public async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
