using FluentValidation;
using FreshBack.Application.AutoMapper.Abstraction;
using FreshBack.Application.AutoMapper.Addresses;
using FreshBack.Application.AutoMapper.Branches;
using FreshBack.Application.AutoMapper.Carts;
using FreshBack.Application.AutoMapper.Categories;
using FreshBack.Application.AutoMapper.Customers;
using FreshBack.Application.AutoMapper.CustomersBranchesFavorite;
using FreshBack.Application.AutoMapper.Merchants;
using FreshBack.Application.AutoMapper.Notifications;
using FreshBack.Application.AutoMapper.Orders;
using FreshBack.Application.AutoMapper.OrdersPhotos;
using FreshBack.Application.AutoMapper.OtpCodes;
using FreshBack.Application.AutoMapper.Products;
using FreshBack.Application.AutoMapper.ProductsOrders;
using FreshBack.Application.AutoMapper.Roles;
using FreshBack.Application.AutoMapper.Settings.Areas;
using FreshBack.Application.AutoMapper.Settings.Users;
using FreshBack.Application.AutoMapper.Shared;
using FreshBack.Application.Configurations;
using FreshBack.Application.Interfaces.Abstraction;
using FreshBack.Application.Interfaces.Addresses;
using FreshBack.Application.Interfaces.Branches;
using FreshBack.Application.Interfaces.Carts;
using FreshBack.Application.Interfaces.Categories;
using FreshBack.Application.Interfaces.Customers;
using FreshBack.Application.Interfaces.CustomersBranchesFavorite;
using FreshBack.Application.Interfaces.FinanceManagement;
using FreshBack.Application.Interfaces.Merchants;
using FreshBack.Application.Interfaces.Notifications;
using FreshBack.Application.Interfaces.Orders;
using FreshBack.Application.Interfaces.OrdersPhotos;
using FreshBack.Application.Interfaces.OtpCodes;
using FreshBack.Application.Interfaces.Products;
using FreshBack.Application.Interfaces.ProductsOrders;
using FreshBack.Application.Interfaces.Roles;
using FreshBack.Application.Interfaces.Settings.Areas;
using FreshBack.Application.Interfaces.Settings.Users;
using FreshBack.Application.Interfaces.Shared;
using FreshBack.Application.Services.Abstraction;
using FreshBack.Application.Services.Addresses;
using FreshBack.Application.Services.Branches;
using FreshBack.Application.Services.Carts;
using FreshBack.Application.Services.Categories;
using FreshBack.Application.Services.Customers;
using FreshBack.Application.Services.CustomersBranchesFavorite;
using FreshBack.Application.Services.FinanceManagement;
using FreshBack.Application.Services.Merchants;
using FreshBack.Application.Services.Notifications;
using FreshBack.Application.Services.Orders;
using FreshBack.Application.Services.OrdersPhotos;
using FreshBack.Application.Services.OtpCodes;
using FreshBack.Application.Services.Products;
using FreshBack.Application.Services.ProductsOrders;
using FreshBack.Application.Services.Roles;
using FreshBack.Application.Services.Settings.Areas;
using FreshBack.Application.Services.Shared;
using FreshBack.Application.Services.Users;
using FreshBack.Application.SignalR.Notifications;
using FreshBack.Application.Validators.Addresses;
using FreshBack.Application.Validators.Branches;
using FreshBack.Application.Validators.Carts;
using FreshBack.Application.Validators.Categories;
using FreshBack.Application.Validators.Customers;
using FreshBack.Application.Validators.CustomersBranchesFavorite;
using FreshBack.Application.Validators.Merchants;
using FreshBack.Application.Validators.Notifications;
using FreshBack.Application.Validators.Orders;
using FreshBack.Application.Validators.OrdersPhotos;
using FreshBack.Application.Validators.OtpCodes;
using FreshBack.Application.Validators.Products;
using FreshBack.Application.Validators.ProductsOrders;
using FreshBack.Application.Validators.Roles;
using FreshBack.Application.Validators.Settings.Areas;
using FreshBack.Application.Validators.Settings.Users;
using FreshBack.Common.Tokens.Configurations;
using FreshBack.Common.Tokens.Interfaces;
using FreshBack.Common.Tokens.Services;
using FreshBack.Domain.Constants;
using FreshBack.Domain.Interfaces.Repositories.Abstraction;
using FreshBack.Domain.Interfaces.Repositories.Addresses;
using FreshBack.Domain.Interfaces.Repositories.Branches;
using FreshBack.Domain.Interfaces.Repositories.Carts;
using FreshBack.Domain.Interfaces.Repositories.Categories;
using FreshBack.Domain.Interfaces.Repositories.Customers;
using FreshBack.Domain.Interfaces.Repositories.CustomersBranchesFavorite;
using FreshBack.Domain.Interfaces.Repositories.Merchants;
using FreshBack.Domain.Interfaces.Repositories.Notifications;
using FreshBack.Domain.Interfaces.Repositories.Orders;
using FreshBack.Domain.Interfaces.Repositories.OrdersPhotos;
using FreshBack.Domain.Interfaces.Repositories.OtpCodes;
using FreshBack.Domain.Interfaces.Repositories.Products;
using FreshBack.Domain.Interfaces.Repositories.ProductsOrders;
using FreshBack.Domain.Interfaces.Repositories.Roles;
using FreshBack.Domain.Interfaces.Repositories.Settings.Areas;
using FreshBack.Domain.Interfaces.Repositories.Settings.Users;
using FreshBack.Domain.Interfaces.Seeders;
using FreshBack.Domain.Interfaces.Specifications.Absraction;
using FreshBack.Domain.Interfaces.UnitOfWork;
using FreshBack.Domain.Models.Roles;
using FreshBack.Domain.Models.Settings.Users;
using FreshBack.Domain.Specifications.Absraction;
using FreshBack.Infrastructure.Data.Context;
using FreshBack.Infrastructure.Data.Repositories.Abstraction;
using FreshBack.Infrastructure.Data.Repositories.Addresses;
using FreshBack.Infrastructure.Data.Repositories.Branches;
using FreshBack.Infrastructure.Data.Repositories.Carts;
using FreshBack.Infrastructure.Data.Repositories.Categories;
using FreshBack.Infrastructure.Data.Repositories.Customers;
using FreshBack.Infrastructure.Data.Repositories.CustomersBranchesFavorite;
using FreshBack.Infrastructure.Data.Repositories.Merchants;
using FreshBack.Infrastructure.Data.Repositories.Notifications;
using FreshBack.Infrastructure.Data.Repositories.Orders;
using FreshBack.Infrastructure.Data.Repositories.OrdersPhotos;
using FreshBack.Infrastructure.Data.Repositories.OtpCodes;
using FreshBack.Infrastructure.Data.Repositories.Products;
using FreshBack.Infrastructure.Data.Repositories.ProductsOrders;
using FreshBack.Infrastructure.Data.Repositories.Roles;
using FreshBack.Infrastructure.Data.Repositories.Settings.Areas;
using FreshBack.Infrastructure.Data.Repositories.Users;
using FreshBack.Infrastructure.Data.Seeders;
using FreshBack.Infrastructure.Data.UnitOfWork;
using FreshBack.WebApi.Middlewares.Exceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FreshBack.Infrastructure.IoC.DependencyContainer;

public static class DependencyContainer
{
    public static void RegisterConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtTokenSettings>(configuration.GetSection(JwtTokenSettings.SectionName));

        services.Configure<ImageSettings>(configuration.GetSection(ImageSettings.SectionName));
    }

    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IBaseService<,,,,,>), typeof(BaseService<,,,,,>))
            .AddScoped<ITokensService, TokensService>()
            .AddScoped<IUserService, UserService>()
            .AddScoped<IRoleService, RoleService>()
            .AddScoped<IAreaService, AreaService>()
            .AddScoped<IReviewService, ReviewService>()
            .AddScoped<IMerchantService, MerchantService>()
            .AddScoped<IBranchService, BranchService>()
            .AddScoped<IProductService, ProductService>()
            .AddScoped<ICartItemService, CartItemService>()
            .AddScoped<ICartService, CartService>()
            .AddScoped<IOrderService, OrderService>()
            .AddScoped<IOrderPhotoService, OrderPhotoService>()
            .AddScoped<IProductOrderService, ProductOrderService>()
            .AddScoped<INotificationService, NotificationService>()
            .AddScoped<IAddressService, AddressService>()
            .AddScoped<ICustomerService, CustomerService>()
            .AddScoped<IOtpCodeService, OtpCodeService>()
            .AddScoped<ICategoryService, CategoryService>()
            .AddScoped<ICustomerBranchFavoriteService, CustomerBranchFavoriteService>()
            .AddScoped<IFinanceManagementService, FinanceManagementService>()
            .AddScoped<IImageService, ImageService>();
    }

    public static void RegisterDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<FreshBackDbContext>(options =>
        {
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sqlOptions => sqlOptions.UseNetTopologySuite());
        });
    }

    public static void RegisterRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IBaseRepository<,>), typeof(BaseRepository<,>))
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IRoleRepository, RoleRepository>()
            .AddScoped<IAreaRepository, AreaRepository>()
            .AddScoped<IReviewRepository, ReviewRepository>()
            .AddScoped<IMerchantRepository, MerchantRepository>()
            .AddScoped<IBranchRepository, BranchRepository>()
            .AddScoped<IProductRepository, ProductRepository>()
            .AddScoped<ICartItemRepository, CartItemRepository>()
            .AddScoped<ICartRepository, CartRepository>()
            .AddScoped<IOrderRepository, OrderRepository>()
            .AddScoped<IOrderPhotoRepository, OrderPhotoRepository>()
            .AddScoped<IProductOrderRepository, ProductOrderRepository>()
            .AddScoped<INotificationRepository, NotificationRepository>()
            .AddScoped<IOtpCodeRepository, OtpCodeRepository>()
            .AddScoped<IAddressRepository, AddressRepository>()
            .AddScoped<ICustomerRepository, CustomerRepository>()
            .AddScoped<ICustomerBranchFavoriteRepository,
                CustomerBranchFavoriteRepository>()
            .AddScoped<ICategoryRepository, CategoryRepository>();
    }

    public static void RegisterSpecifications(this IServiceCollection services)
    {
        services.AddScoped(typeof(IBaseSpecification<>), typeof(BaseSpecification<>))
            .AddScoped(typeof(ISpecificationCombiner<>), typeof(SpecificationCombiner<>));
    }

    public static void RegisterUnitOfWork(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    public static void RegisterAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(BaseModelProfile).Assembly);
        services.AddAutoMapper(typeof(PaginatedModelProfile).Assembly);
        services.AddAutoMapper(typeof(UserProfile).Assembly);
        services.AddAutoMapper(typeof(RoleProfile).Assembly);
        services.AddAutoMapper(typeof(AreaProfile).Assembly);
        services.AddAutoMapper(typeof(ReviewProfile).Assembly);
        services.AddAutoMapper(typeof(MerchantProfile).Assembly);
        services.AddAutoMapper(typeof(BranchProfile).Assembly);
        services.AddAutoMapper(typeof(ProductProfile).Assembly);
        services.AddAutoMapper(typeof(CartItemProfile).Assembly);
        services.AddAutoMapper(typeof(CartProfile).Assembly);
        services.AddAutoMapper(typeof(OrderProfile).Assembly);
        services.AddAutoMapper(typeof(OrderPhotoProfile).Assembly);
        services.AddAutoMapper(typeof(ProductOrderProfile).Assembly);
        services.AddAutoMapper(typeof(NotificationProfile).Assembly);
        services.AddAutoMapper(typeof(AddressProfile).Assembly);
        services.AddAutoMapper(typeof(CustomerProfile).Assembly);
        services.AddAutoMapper(typeof(OtpCodeProfile).Assembly);
        services.AddAutoMapper(typeof(CategoryProfile).Assembly);
        services.AddAutoMapper(typeof(CustomerBranchFavoriteProfile).Assembly);
    }

    public static void RegisterValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<UserDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<LoginDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<ResetPasswordDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<ForgotPasswordDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<RoleDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<AreaDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<ReviewDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<MerchantDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<BranchDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<ProductDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<CartItemDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<CartDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<OrderDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<OrderPhotoDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<CreateOrderPhotoDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<ProductOrderDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<NotificationDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<AddressDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<CustomerDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<CreateCustomerDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<OtpCodeDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<CategoryDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<
            CreateCustomerBranchFavoriteDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<
            CustomerBranchFavoriteDtoValidator>();
    }

    public static void RegisterIdentity(this IServiceCollection services)
    {
        services.AddIdentity<User, Role>()
            .AddEntityFrameworkStores<FreshBackDbContext>()
            .AddDefaultTokenProviders();
    }

    public static void RegisterJwtSettings(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("JwtTokens");
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["ValidIssuer"],
                ValidAudience = jwtSettings["ValidAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!)),
                ClockSkew = new TimeSpan(0, 2, 0)
            };
        });
    }

    public static void RegisterCORS(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(options =>
        {
            var allowedOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()!;

            options.AddPolicy(AppSettings.AllowedOrigins,
                policy => policy.WithOrigins(allowedOrigins)
                                .AllowAnyMethod()
                                .AllowAnyHeader()
                                .AllowCredentials());
        });
    }

    public static void ApplyMigrations(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<FreshBackDbContext>();

        dbContext.Database.Migrate();
    }

    public static void RegisterMiddlewares(this IServiceCollection services)
    {
        services.AddTransient<ExceptionHandlingMiddleware>();
    }

    public static IServiceCollection RegisterDatabaseSeeder(this IServiceCollection services)
    {
        services.AddScoped<IDatabaseSeeder, DatabaseSeeder>();

        return services;
    }

    public static void RegisterSignalR(this IServiceCollection services)
    {
        services.AddSignalR();

        services.AddScoped<INotificationSender, SignalRNotificationSender>();
    }

    public static async Task SeedDatabaseAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var services = scope.ServiceProvider;
        var seeder = services.GetRequiredService<IDatabaseSeeder>();

        await seeder.SeedAsync();
    }
}
