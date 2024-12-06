using Ambev.DeveloperEvaluation.Domain.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Ambev.DeveloperEvaluation.ORM.Repositories.Generic
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbContext _context;
        private readonly ILogger<Repository<T>> _logger;

        public Repository(DbContext context, ILogger<Repository<T>> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            _logger.LogInformation("Retrieving entity with ID {Id} from the database.", id);
            try
            {
                using (_logger.BeginScope("Method: GetByIdAsync"))
                {
                    using var dbContext = _context;
                    var entity = await dbContext.Set<T>().FindAsync(id);
                    if (entity == null)
                    {
                        _logger.LogWarning("Entity with ID {Id} not found.", id);
                    }
                    else
                    {
                        _logger.LogInformation("Entity with ID {Id} retrieved successfully.", id);
                    }
                    return entity;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving entity with ID {Id}.", id);
                throw;
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            _logger.LogInformation("Retrieving all entities from the database.");
            try
            {
                using (_logger.BeginScope("Method: GetAllAsync"))
                {
                    using var dbContext = _context;
                    var entities = await dbContext.Set<T>().ToListAsync();
                    _logger.LogInformation("Successfully retrieved {Count} entities.", entities.Count);
                    return entities;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all entities.");
                throw;
            }
        }

        public async Task AddAsync(T entity)
        {
            _logger.LogInformation("Adding a new entity to the database: {Entity}.", entity);
            try
            {
                using (_logger.BeginScope("Method: AddAsync"))
                {
                    using var dbContext = _context;
                    await dbContext.Set<T>().AddAsync(entity);
                    await dbContext.SaveChangesAsync();
                    _logger.LogInformation("Entity added successfully.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding the entity: {Entity}.", entity);
                throw;
            }
        }

        public async Task UpdateAsync(T entity)
        {
            _logger.LogInformation("Updating an entity in the database: {Entity}.", entity);
            try
            {
                using (_logger.BeginScope("Method: UpdateAsync"))
                {
                    using var dbContext = _context;
                    dbContext.Set<T>().Update(entity);
                    await dbContext.SaveChangesAsync();
                    _logger.LogInformation("Entity updated successfully.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the entity: {Entity}.", entity);
                throw;
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            _logger.LogInformation("Deleting entity with ID {Id} from the database.", id);
            try
            {
                using (_logger.BeginScope("Method: DeleteAsync"))
                {
                    using var dbContext = _context;
                    var entity = await dbContext.Set<T>().FindAsync(id);
                    if (entity == null)
                    {
                        _logger.LogWarning("Entity with ID {Id} not found. Deletion skipped.", id);
                        return;
                    }

                    dbContext.Set<T>().Remove(entity);
                    await dbContext.SaveChangesAsync();
                    _logger.LogInformation("Entity with ID {Id} deleted successfully.", id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the entity with ID {Id}.", id);
                throw;
            }
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            _logger.LogInformation("Searching for entities with a given predicate.");
            try
            {
                using (_logger.BeginScope("Method: FindAsync"))
                {
                    using var dbContext = _context;
                    var results = await dbContext.Set<T>().Where(predicate).ToListAsync();
                    _logger.LogInformation("Successfully found {Count} matching entities.", results.Count);
                    return results;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while searching for entities.");
                throw;
            }
        }
    }
}
