using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Services.Interfaces;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSales
{
    public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand,UpdateSaleResult>
    {
        private readonly IRepository<Sale> _saleRepository;
        private readonly ISaleService _saleService;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateSaleHandler> _logger;

        public UpdateSaleHandler(IRepository<Sale> saleRepository, ISaleService saleService, ILogger<UpdateSaleHandler> logger , IMapper mapper)
        {
            _saleRepository = saleRepository;
            _saleService = saleService;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<UpdateSaleResult> Handle(UpdateSaleCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating Sale with ID: {SaleId}", command.Id);

            var validator = new UpdateSaleCommandValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var existingSale = await _saleRepository.GetByIdAsync(command.Id);

            if (existingSale == null)
            {
                _logger.LogWarning("Sale with ID: {SaleId} not found", command.Id);
                throw new KeyNotFoundException($"Sale with ID {command.Id} not found");
            }

            var newSalesData = _mapper.Map<Sale>(command);
            
            validationResult = _saleService.ValidateUpdateSale(existingSale, newSalesData);
          
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed for Sale with ID: {SaleId}", command.Id);
                throw new ValidationException(validationResult.Errors);
            }

            await _saleRepository.UpdateAsync(newSalesData);

            _logger.LogInformation("Sale successfully updated with ID: {SaleId}", newSalesData.Id);

            var result = _mapper.Map<UpdateSaleResult>(newSalesData);

            return result;
        }
    }

}
