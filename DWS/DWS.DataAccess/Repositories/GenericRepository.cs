using DWS.DataAccess.Context;
using DWS.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DWS.DataAccess.Repositories;

// Generic Repository class for performing common database operations.
// TEntity represents any entity class in the application.
public class GenericRepository<TEntity> : IGenericRepository<TEntity>
    where TEntity : class
{
    // DbSet provides access to the database table of the given entity.
    private readonly DbSet<TEntity> _dbSet;

    // Constructor: Receives ApplicationDbContext through dependency injection.
    public GenericRepository(ApplicationDbContext context)
    {
        // Gets the DbSet associated with the specified entity type.
        _dbSet = context.Set<TEntity>();
    }

    // Retrieves a single entity using its primary key.
    public async Task<TEntity?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.FindAsync(
            new object?[] { id },
            cancellationToken);
    }

    // Retrieves all records from the corresponding database table.
    // AsNoTracking improves read-only query performance.
    public async Task<IReadOnlyList<TEntity>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    // Adds a new entity to the database context.
    // The actual database save will be handled by Unit of Work.
    public async Task AddAsync(
        TEntity entity,
        CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
    }

    // Marks an existing entity as modified.
    // Changes will be saved through Unit of Work.
    public void Update(TEntity entity)
    {
        _dbSet.Update(entity);
    }

    // Marks an entity for deletion from the database.
    // The deletion will be committed through Unit of Work.
    public void Delete(TEntity entity)
    {
        _dbSet.Remove(entity);
    }
}