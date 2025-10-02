
using RealEstate.Application.Modules.Owner.Services;
using RealEstate.Application.Modules.Owner.Mappings;
using RealEstate.Domain.Modules.Owner.Interfaces;
using RealEstate.Infrastructure.Modules.Owner.Repositories;
using RealEstate.Infrastructure.Common.Configuration;
using Microsoft.OpenApi.Models;
using System.Reflection;
using RealEstate.Infrastructure.Common.Data;
using RealEstate.Application.Modules.Owner.Interfaces;
using RealEstate.Application.Modules.Property.Services;
using RealEstate.Application.Modules.Property.Mappings;
using RealEstate.Domain.Modules.Property.Interfaces;
using RealEstate.Infrastructure.Modules.Property.Repositories;
using RealEstate.Application.Modules.Property.Interfaces;
using RealEstate.Application.Modules.Property.Validators;
using RealEstate.Application.Modules.Owner.Validators;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure MongoDB
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

// Register MongoDB Context
builder.Services.AddSingleton<MongoDbContext>();

// Register repositories
builder.Services.AddScoped<IOwnerRepository, RealEstate.Infrastructure.Modules.Owner.Repositories.OwnerRepository>();
builder.Services.AddScoped<IPropertyRepository, RealEstate.Infrastructure.Modules.Property.Repositories.PropertyRepository>();

// Register services
builder.Services.AddScoped<IOwnerService, OwnerService>();
builder.Services.AddScoped<IPropertyService, PropertyService>();

// Register AutoMapper
builder.Services.AddAutoMapper(typeof(OwnerProfile), typeof(PropertyProfile));

// Register FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<CreatePropertyValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdatePropertyValidator>();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000", "http://localhost:3001")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Real Estate API",
        Version = "v1",
        Description = "A comprehensive API for managing real estate properties with full CRUD operations and advanced filtering capabilities.",
        Contact = new OpenApiContact
        {
            Name = "Real Estate Team",
            Email = "contact@realestate.com"
        },
        License = new OpenApiLicense
        {
            Name = "MIT License",
            Url = new Uri("https://opensource.org/licenses/MIT")
        }
    });

    // Include XML comments for better documentation
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

    // Add examples and schema filters
    c.EnableAnnotations();
    c.UseInlineDefinitionsForEnums();

    // Add operation filters for better documentation
    c.DescribeAllParametersInCamelCase();
});

// Add health checks
builder.Services.AddHealthChecks();

// Add logging
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddConsole();
    loggingBuilder.AddDebug();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Real Estate API v1");
        c.RoutePrefix = string.Empty;
    });
}

// Enable CORS
app.UseCors("AllowReactApp");

// Add security headers
app.Use(async (context, next) =>
{
    context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Append("X-Frame-Options", "DENY");
    context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");
    await next();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Add health check endpoint
app.MapHealthChecks("/health");

app.Run();