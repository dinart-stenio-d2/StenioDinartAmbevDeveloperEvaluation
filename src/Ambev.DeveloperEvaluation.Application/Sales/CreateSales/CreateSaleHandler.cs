using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Services.Interfaces;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSales
{
    public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
    {
        private readonly IRepository<Sale> _saleRepository;
        private readonly ISaleService _saleService;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateSaleHandler> _logger;

        public CreateSaleHandler(IRepository<Sale> saleRepository, ISaleService saleService, ILogger<CreateSaleHandler> logger , IMapper mapper)
        {
            _saleRepository = saleRepository;
            _saleService = saleService;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<CreateSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting CreateSaleHandler for SaleNumber: {SaleNumber}", command.SaleNumber);


            var validator = new CreateSaleCommandValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);
            try
            {
                var sale = _mapper.Map<Sale>(command);

                sale.GenerateSaleNumber();
                sale.RegenerateId();
                foreach (var item in sale.Items)
                {
                    item.RegenerateId();
                }

                var saleValidationResult = _saleService.ValidateSale(sale);
                if (!saleValidationResult.IsValid)
                {
                    _logger.LogWarning("Validation failed for SaleNumber: {SaleNumber}", command.SaleNumber);
                    throw new ValidationException(saleValidationResult.Errors);
                }

                await _saleRepository.AddAsync(sale);

                _logger.LogInformation("Sale successfully created with SaleNumber: {SaleNumber}", sale.SaleNumber);

                var result = _mapper.Map<CreateSaleResult>(sale);

                return result;
            }
            catch (Exception ex)
            {
                throw new ValidationException(ex.Message);
            }

           
        }
    }
}
