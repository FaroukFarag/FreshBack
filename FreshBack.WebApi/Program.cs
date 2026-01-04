using FluentValidation.AspNetCore;
using FreshBack.Application.SignalR.Notifications;
using FreshBack.Domain.Constants;
using FreshBack.Infrastructure.IoC.DependencyContainer;
using FreshBack.WebApi.Middlewares.Exceptions;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "HR Management System API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer <your-token>' below to authenticate."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            []
        }
    });
});

builder.Services.RegisterDbContext(builder.Configuration);
builder.Services.RegisterConfiguration(builder.Configuration);
builder.Services.RegisterServices();
builder.Services.RegisterSpecifications();
builder.Services.RegisterRepositories();
builder.Services.RegisterUnitOfWork();
builder.Services.RegisterAutoMapper();
builder.Services.RegisterValidators();
builder.Services.RegisterIdentity();
builder.Services.RegisterJwtSettings(builder.Configuration);
builder.Services.RegisterCORS(builder.Configuration);
builder.Services.RegisterMiddlewares();
builder.Services.RegisterSignalR();

var app = builder.Build();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "Images")),
    RequestPath = "/Images"
});

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

app.Services.ApplyMigrations();

app.UseCors(AppSettings.AllowedOrigins);

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.MapHub<NotificationHub>("/hubs/notifications");

app.Run();
