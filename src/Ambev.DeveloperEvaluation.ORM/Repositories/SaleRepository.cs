using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    public class SaleRepository : IRepository<Sale>
    {
        private readonly IRepository<Sale> _repository;
        private readonly ILogger<SaleRepository> _logger;

        public SaleRepository(IRepository<Sale> repository, ILogger<SaleRepository> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Sale?> GetByIdAsync(Guid id)
        {
            _logger.LogInformation("Retrieving sale with ID {Id} from the repository.", id);
            try
            {
                var sale = await _repository.GetByIdAsync(id);
                if (sale == null)
                {
                    _logger.LogWarning("No sale found with ID {Id}.", id);
                }
                else
                {
                    _logger.LogInformation("Successfully retrieved sale with ID {Id}.", id);
                }
                return sale;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the sale with ID {Id}.", id);
                throw;
            }
        }

        public async Task<IEnumerable<Sale>> GetAllAsync()
        {
            _logger.LogInformation("Retrieving all sales from the repository.");
            try
            {
                var sales = await _repository.GetAllAsync();
                _logger.LogInformation("Successfully retrieved {Count} sales.", sales.Count());
                return sales;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all sales.");
                throw;
            }
        }

        public async Task AddAsync(Sale sale)
        {
            _logger.LogInformation("Adding a new sale to the repository: {Sale}.", sale);
            try
            {
                await _repository.AddAsync(sale);
                _logger.LogInformation("Successfully added sale with ID {Id}.", sale.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding the sale: {Sale}.", sale);
                throw;
            }
        }

        public async Task UpdateAsync(Sale sale)
        {
            _logger.LogInformation("Updating sale with ID {Id} in the repository.", sale.Id);
            try
            {
                await _repository.UpdateAsync(sale);
                _logger.LogInformation("Successfully updated sale with ID {Id}.", sale.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the sale with ID {Id}.", sale.Id);
                throw;
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            _logger.LogInformation("Deleting sale with ID {Id} from the repository.", id);
            try
            {
                await _repository.DeleteAsync(id);
                _logger.LogInformation("Successfully deleted sale with ID {Id}.", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the sale with ID {Id}.", id);
                throw;
            }
        }

        public async Task<IEnumerable<Sale>> FindAsync(System.Linq.Expressions.Expression<Func<Sale, bool>> predicate)
        {
            _logger.LogInformation("Searching sales using a predicate.");
            try
            {
                var results = await _repository.FindAsync(predicate);
                _logger.LogInformation("Successfully found {Count} matching sales.", results.Count());
                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while searching for sales.");
                throw;
            }
        }
    }

}
