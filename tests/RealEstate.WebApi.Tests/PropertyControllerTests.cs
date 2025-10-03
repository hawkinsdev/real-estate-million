using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using RealEstate.Application.Modules.Property.DTOs;
using RealEstate.Application.Modules.Property.Interfaces;
using RealEstate.WebApi.Controllers;

namespace RealEstate.WebApi.Tests;

[TestFixture]
public class PropertyControllerTests
{
    private Mock<IPropertyService> _mockPropertyService;
    private Mock<ILogger<PropertyController>> _mockLogger;
    private PropertyController _controller;

    [SetUp]
    public void Setup()
    {
        _mockPropertyService = new Mock<IPropertyService>();
        _mockLogger = new Mock<ILogger<PropertyController>>();
        _controller = new PropertyController(_mockPropertyService.Object, _mockLogger.Object);
    }

    [Test]
    public async Task GetAllProperties_ShouldReturnOkResult()
    {
        // Arrange
        var properties = new List<PropertyDto>
        {
            new PropertyDto { IdProperty = "1", Name = "Test Property 1", Price = 100000 },
            new PropertyDto { IdProperty = "2", Name = "Test Property 2", Price = 200000 }
        };

        _mockPropertyService.Setup(s => s.GetAllPropertiesAsync())
            .ReturnsAsync(properties);

        // Act
        var result = await _controller.GetAllProperties();

        // Assert
        Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        var okResult = result.Result as OkObjectResult;
        Assert.That(okResult?.Value, Is.EqualTo(properties));
    }


    [Test]
    public async Task GetPropertyById_WithValidId_ShouldReturnOkResult()
    {
        // Arrange
        var propertyId = "1";
        var property = new PropertyDto { IdProperty = propertyId, Name = "Test Property", Price = 100000 };

        _mockPropertyService.Setup(s => s.GetPropertyByIdAsync(propertyId))
            .ReturnsAsync(property);

        // Act
        var result = await _controller.GetPropertyById(propertyId);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        var okResult = result.Result as OkObjectResult;
        Assert.That(okResult?.Value, Is.EqualTo(property));
    }

    [Test]
    public async Task GetPropertyById_WithInvalidId_ShouldReturnBadRequest()
    {
        // Arrange
        var invalidId = "";

        // Act
        var result = await _controller.GetPropertyById(invalidId);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        var badRequestResult = result.Result as BadRequestObjectResult;
        Assert.That(badRequestResult?.Value, Is.EqualTo("Property ID is required"));
    }

    [Test]
    public async Task GetPropertyById_WithNonExistentId_ShouldReturnNotFound()
    {
        // Arrange
        var propertyId = "999";
        _mockPropertyService.Setup(s => s.GetPropertyByIdAsync(propertyId))
            .ReturnsAsync((PropertyDto?)null);

        // Act
        var result = await _controller.GetPropertyById(propertyId);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
        var notFoundResult = result.Result as NotFoundObjectResult;
        Assert.That(notFoundResult?.Value, Is.EqualTo($"Property with ID {propertyId} not found"));
    }

    [Test]
    public async Task SearchProperties_WithValidFilters_ShouldReturnOkResult()
    {
        // Arrange
        var filters = new PropertyFilterDto
        {
            Name = "Test",
            MinPrice = 100000,
            MaxPrice = 500000
        };

        var properties = new List<PropertyDto>
        {
            new PropertyDto { IdProperty = "1", Name = "Test Property", Price = 200000 }
        };

        _mockPropertyService.Setup(s => s.GetFilteredPropertiesAsync(It.IsAny<PropertyFilterDto>()))
            .ReturnsAsync(properties);

        // Act
        var result = await _controller.SearchProperties("Test", null, 100000, 500000);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        var okResult = result.Result as OkObjectResult;
        Assert.That(okResult?.Value, Is.EqualTo(properties));
    }

    [Test]
    public async Task SearchProperties_WithInvalidPriceRange_ShouldReturnBadRequest()
    {
        // Arrange
        var minPrice = 500000m;
        var maxPrice = 100000m;

        // Act
        var result = await _controller.SearchProperties(null, null, minPrice, maxPrice);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        var badRequestResult = result.Result as BadRequestObjectResult;
        Assert.That(badRequestResult?.Value, Is.EqualTo("Minimum price cannot be greater than maximum price"));
    }

    [Test]
    public async Task SearchProperties_WithNegativeMinPrice_ShouldReturnBadRequest()
    {
        // Arrange
        var minPrice = -1000m;

        // Act
        var result = await _controller.SearchProperties(null, null, minPrice, null);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        var badRequestResult = result.Result as BadRequestObjectResult;
        Assert.That(badRequestResult?.Value, Is.EqualTo("Minimum price cannot be negative"));
    }






}
