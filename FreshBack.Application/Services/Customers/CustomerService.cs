using AutoMapper;
using FreshBack.Application.Dtos.Customers;
using FreshBack.Application.Dtos.Shared;
using FreshBack.Application.Interfaces.Customers;
using FreshBack.Application.Services.Abstraction;
using FreshBack.Common.Tokens.Interfaces;
using FreshBack.Domain.Enums.Roles;
using FreshBack.Domain.Interfaces.Repositories.Customers;
using FreshBack.Domain.Interfaces.UnitOfWork;
using FreshBack.Domain.Models.Customers;
using FreshBack.Domain.Specifications.Absraction;
using System.Security.Claims;

namespace FreshBack.Application.Services.Customers;

public class CustomerService(
    ICustomerRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ITokensService tokensService) : BaseService<
    CreateCustomerDto,
    CustomerDto,
    CustomerDto,
    CustomerDto,
    Customer,
    int>(repository, unitOfWork, mapper), ICustomerService
{
    private readonly ICustomerRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly ITokensService _tokensService = tokensService;

    public async override Task<ResultDto<CreateCustomerDto>> CreateAsync(
        CreateCustomerDto createCustomerDto)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Create Customer",
            action: async () =>
            {
                var spec = new BaseSpecification<Customer>
                {
                    Criteria = c => c.PhoneNumber == createCustomerDto.PhoneNumber
                };
                var existingCustomers = await _repository.GetAllAsync(spec);

                if (existingCustomers.Any())
                    throw new Exception("This number already registered, login instead");

                var customer = _mapper.Map<Customer>(createCustomerDto);

                await _repository.CreateAsync(customer);

                var customerCreated = await _unitOfWork.Complete();

                if (!customerCreated)
                    throw new Exception("Failed to create customer");

                createCustomerDto = _mapper.Map<CreateCustomerDto>(customer);

                var claims = new List<TokenClaim>
                {
                    new("customerId", createCustomerDto.Id.ToString()),
                    new("email", createCustomerDto.Email ?? string.Empty),
                    new(ClaimTypes.Role, RoleNames.Customer.ToString())
                };

                createCustomerDto.Token = await _tokensService.GenerateToken(claims);

                return createCustomerDto;
            });
    }

    public async Task<ResultDto<CreateCustomerDto>> LoginAsync(
        LoginCustomerDto loginCustomerDto)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Login Customer",
            action: async () =>
            {
                var spec = new BaseSpecification<Customer>
                {
                    Criteria = c => c.PhoneNumber == loginCustomerDto.PhoneNumber
                };
                var existingCustomers = await _repository.GetAllAsync(spec);

                if (!existingCustomers.Any())
                    throw new Exception("This number doesn't exist, register now");

                var createCustomerDto = _mapper.Map<CreateCustomerDto>(existingCustomers
                    .FirstOrDefault());
                var claims = new List<TokenClaim>
                {
                    new("customerId", createCustomerDto.Id.ToString()),
                    new("email", createCustomerDto.Email ?? string.Empty)
                };

                createCustomerDto.Token = await _tokensService.GenerateToken(claims);

                return createCustomerDto;
            });
    }
}
