using Ambev.DeveloperEvaluation.Application.Sales.CreateSales;
using Ambev.DeveloperEvaluation.Application.Sales.DeleteSales;
using Ambev.DeveloperEvaluation.Application.Sales.GetAllSales;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSales;
using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetAllSales;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<SalesController> _logger;

        /// <summary>
        /// Initializes a new instance of SalesController
        /// </summary>
        /// <param name="mediator">The mediator instance</param>
        /// <param name="mapper">The AutoMapper instance</param>
        /// <param name="logger">The logger instance</param>
        public SalesController(IMediator mediator, IMapper mapper, ILogger<SalesController> logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseWithData<CreateSaleResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateSale([FromBody] CreateSaleRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Received request to create a sale.");
            try
            {
                var validator = new CreateSaleRequestValidator();
                var validationResult = await validator.ValidateAsync(request, cancellationToken);

                if (!validationResult.IsValid)
                {
                    _logger.LogWarning("Validation failed for CreateSaleRequest: {Errors}", validationResult.Errors);
                    return BadRequest(validationResult.Errors);
                }

                var command = _mapper.Map<CreateSaleCommand>(request);
                var response = await _mediator.Send(command, cancellationToken);

                _logger.LogInformation("Sale created successfully.");
                return Created(string.Empty, new ApiResponseWithData<CreateSaleResponse>
                {
                    Success = true,
                    Message = "Sale created successfully",
                    Data = _mapper.Map<CreateSaleResponse>(response)
                });
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning("Validation error while updatating sale: {Errors}", ex.Errors);
                var listErrors = new List<ValidationErrorDetail>
                {
                    new ValidationErrorDetail
                    {
                        Error = ex.Message
                    }
                };
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Validation error occurred.",
                    Errors = listErrors
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while creating sale.");
                var listErrors = new List<ValidationErrorDetail>
                {
                    new ValidationErrorDetail
                    {
                        Error = ex.Message
                    }
                };
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse
                {
                    Success = false,
                    Message = "An unexpected error occurred.",
                    Errors = listErrors
                });
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponseWithData<GetSaleResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSale([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Received request to retrieve sale with ID: {SaleId}", id);
            try
            {
                var request = new GetSaleRequest { Id = id };
                var validator = new GetSaleRequestValidator();
                var validationResult = await validator.ValidateAsync(request, cancellationToken);

                if (!validationResult.IsValid)
                {
                    _logger.LogWarning("Validation failed for GetSaleRequest: {Errors}", validationResult.Errors);
                    return BadRequest(validationResult.Errors);
                }

                var command = _mapper.Map<GetSaleCommand>(request.Id);
                var response = await _mediator.Send(command, cancellationToken);

                _logger.LogInformation("Sale retrieved successfully with ID: {SaleId}", id);
                return Ok(new ApiResponseWithData<GetSaleResponse>
                {
                    Success = true,
                    Message = "Sale retrieved successfully",
                    Data = _mapper.Map<GetSaleResponse>(response)
                });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Sale not found: {Message}", ex.Message);
                return NotFound(new ApiResponse { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while getting a sale.");
                var listErrors = new List<ValidationErrorDetail>
                {
                    new ValidationErrorDetail
                    {
                        Error = ex.Message
                    }
                };
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse
                {
                    Success = false,
                    Message = "An unexpected error occurred.",
                    Errors = listErrors
                });
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponseWithData<IEnumerable<GetAllSalesResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllSales(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Received request to retrieve all sales.");
            try
            {
                var query = new GetAllSalesQuery();
                var response = await _mediator.Send(query, cancellationToken);

                _logger.LogInformation("Successfully retrieved all sales.");
                return Ok(new ApiResponseWithData<IEnumerable<GetAllSalesResponse>>
                {
                    Success = true,
                    Message = "Sales retrieved successfully",
                    Data = _mapper.Map<IEnumerable<GetAllSalesResponse>>(response)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while getting all sales.");
                var listErrors = new List<ValidationErrorDetail>
                {
                    new ValidationErrorDetail
                    {
                        Error = ex.Message
                    }
                };
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse
                {
                    Success = false,
                    Message = "An unexpected error occurred.",
                    Errors = listErrors
                });
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponseWithData<UpdateSaleResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateSale([FromRoute] Guid id, [FromBody] UpdateSaleRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Received request to update sale with ID: {SaleId}", id);
            try
            {
                if (id != request.Id)
                {
                    _logger.LogWarning("Sale ID mismatch: {RouteId} != {RequestId}", id, request.Id);
                    return BadRequest("Sale ID in the route and request body must match.");
                }

                var validator = new UpdateSaleRequestValidator();
                var validationResult = await validator.ValidateAsync(request, cancellationToken);

                if (!validationResult.IsValid)
                {
                    _logger.LogWarning("Validation failed for UpdateSaleRequest: {Errors}", validationResult.Errors);
                    return BadRequest(validationResult.Errors);
                }

                var command = _mapper.Map<UpdateSaleCommand>(request);
                var response = await _mediator.Send(command, cancellationToken);

                _logger.LogInformation("Sale updated successfully with ID: {SaleId}", id);
                return Ok(new ApiResponseWithData<UpdateSaleResponse>
                {
                    Success = true,
                    Message = "Sale updated successfully",
                    Data = _mapper.Map<UpdateSaleResponse>(response)
                });
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning("Validation error while updatating sale: {Errors}", ex.Errors);
                var listErrors = new List<ValidationErrorDetail>
                {
                    new ValidationErrorDetail
                    {
                        Error = ex.Message 
                    }
                };
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Validation error occurred.",
                    Errors = listErrors
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating sale with ID: {SaleId}", id);
                throw;
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponseWithData<DeleteSale.DeleteSaleResponse>), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteSale([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Received request to delete sale with ID: {SaleId}", id);
            try
            {
                dynamic response;
                var request = new DeleteSaleRequest { Id = id };
                var command = _mapper.Map<DeleteSaleCommand>(request);
                response = await _mediator.Send(command, cancellationToken);

            _logger.LogInformation("Sale deleted successfully with ID: {SaleId}", id);

            return NoContent();
            
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning("Validation error while deleting sale: {Errors}", ex.Errors);
                var listErrors = new List<ValidationErrorDetail>
                {
                    new ValidationErrorDetail
                    {
                        Error = ex.Message
                    }
                };
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Validation error occurred.",
                    Errors = listErrors
                });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Sale not found: {Message}", ex.Message);
                var listErrors = new List<ValidationErrorDetail>
                {
                    new ValidationErrorDetail
                    {
                        Error = ex.Message
                    }
                };
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = ex.Message,
                    Errors = listErrors
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while deleting sale.");
                var listErrors = new List<ValidationErrorDetail>
                {
                    new ValidationErrorDetail
                    {
                        Error = ex.Message
                    }
                };
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse
                {
                    Success = false,
                    Message = "An unexpected error occurred.",
                    Errors = listErrors
                });
            }
        }
    }
}
