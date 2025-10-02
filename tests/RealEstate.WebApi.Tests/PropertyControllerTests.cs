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
    public async Task GetAllPropertiesSimple_ShouldReturnOkResult()
    {
        // Arrange
        var properties = new List<PropertySimpleDto>
        {
            new PropertySimpleDto { IdOwner = "owner1", Name = "Test Property 1", Price = 100000, Address = "Test Address 1", Image = "test.jpg" },
            new PropertySimpleDto { IdOwner = "owner2", Name = "Test Property 2", Price = 200000, Address = "Test Address 2", Image = "test2.jpg" }
        };

        _mockPropertyService.Setup(s => s.GetFilteredPropertiesSimpleAsync(It.IsAny<PropertyFilterDto>()))
            .ReturnsAsync(properties);

        // Act
        var result = await _controller.GetAllPropertiesSimple();

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

    [Test]
    public async Task SearchPropertiesSimple_WithValidFilters_ShouldReturnOkResult()
    {
        // Arrange
        var properties = new List<PropertySimpleDto>
        {
            new PropertySimpleDto { IdOwner = "owner1", Name = "Test Property", Price = 200000, Address = "Test Address", Image = "test.jpg" }
        };

        _mockPropertyService.Setup(s => s.GetFilteredPropertiesSimpleAsync(It.IsAny<PropertyFilterDto>()))
            .ReturnsAsync(properties);

        // Act
        var result = await _controller.SearchPropertiesSimple("Test", null, 100000, 500000);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        var okResult = result.Result as OkObjectResult;
        Assert.That(okResult?.Value, Is.EqualTo(properties));
    }

    [Test]
    public async Task CreateProperty_WithValidData_ShouldReturnCreatedResult()
    {
        // Arrange
        var createDto = new CreatePropertyDto
        {
            Name = "New Property",
            Address = "New Address",
            Price = 300000,
            CodeInternal = "NEW-001",
            Year = 2023,
            IdOwner = "owner1"
        };

        var createdProperty = new PropertyDto
        {
            IdProperty = "1",
            Name = createDto.Name,
            Address = createDto.Address,
            Price = createDto.Price,
            CodeInternal = createDto.CodeInternal,
            Year = createDto.Year,
            IdOwner = createDto.IdOwner
        };

        _mockPropertyService.Setup(s => s.CreatePropertyAsync(createDto))
            .ReturnsAsync(createdProperty);

        // Act
        var result = await _controller.CreateProperty(createDto);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<CreatedAtActionResult>());
        var createdResult = result.Result as CreatedAtActionResult;
        Assert.That(createdResult?.Value, Is.EqualTo(createdProperty));
    }

    [Test]
    public async Task UpdateProperty_WithValidData_ShouldReturnNoContent()
    {
        // Arrange
        var propertyId = "1";
        var updateDto = new UpdatePropertyDto
        {
            Name = "Updated Property",
            Price = 400000
        };

        _mockPropertyService.Setup(s => s.UpdatePropertyAsync(propertyId, updateDto))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.UpdateProperty(propertyId, updateDto);

        // Assert
        Assert.That(result, Is.InstanceOf<NoContentResult>());
        _mockPropertyService.Verify(s => s.UpdatePropertyAsync(propertyId, updateDto), Times.Once);
    }

    [Test]
    public async Task UpdateProperty_WithInvalidId_ShouldReturnBadRequest()
    {
        // Arrange
        var invalidId = "";
        var updateDto = new UpdatePropertyDto { Name = "Updated Property" };

        // Act
        var result = await _controller.UpdateProperty(invalidId, updateDto);

        // Assert
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        var badRequestResult = result as BadRequestObjectResult;
        Assert.That(badRequestResult?.Value, Is.EqualTo("Property ID is required"));
    }

    [Test]
    public async Task DeleteProperty_WithValidId_ShouldReturnNoContent()
    {
        // Arrange
        var propertyId = "1";
        _mockPropertyService.Setup(s => s.DeletePropertyAsync(propertyId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteProperty(propertyId);

        // Assert
        Assert.That(result, Is.InstanceOf<NoContentResult>());
        _mockPropertyService.Verify(s => s.DeletePropertyAsync(propertyId), Times.Once);
    }

    [Test]
    public async Task PropertyExists_WithExistingProperty_ShouldReturnOk()
    {
        // Arrange
        var propertyId = "1";
        _mockPropertyService.Setup(s => s.PropertyExistsAsync(propertyId))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.PropertyExists(propertyId);

        // Assert
        Assert.That(result, Is.InstanceOf<OkResult>());
        _mockPropertyService.Verify(s => s.PropertyExistsAsync(propertyId), Times.Once);
    }

    [Test]
    public async Task PropertyExists_WithNonExistentProperty_ShouldReturnNotFound()
    {
        // Arrange
        var propertyId = "999";
        _mockPropertyService.Setup(s => s.PropertyExistsAsync(propertyId))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.PropertyExists(propertyId);

        // Assert
        Assert.That(result, Is.InstanceOf<NotFoundResult>());
        _mockPropertyService.Verify(s => s.PropertyExistsAsync(propertyId), Times.Once);
    }
}
