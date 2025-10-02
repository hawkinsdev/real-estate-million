using Microsoft.Extensions.Logging;
using Moq;
using RealEstate.Application.Modules.Property.DTOs;
using RealEstate.Application.Modules.Property.Interfaces;
using RealEstate.Application.Modules.Property.Services;
using RealEstate.Domain.Modules.Property.Entities;
using RealEstate.Domain.Modules.Property.Interfaces;
using AutoMapper;

namespace RealEstate.Application.Tests;

[TestFixture]
public class PropertyServiceTests
{
    private Mock<IPropertyRepository> _mockRepository;
    private Mock<IMapper> _mockMapper;
    private PropertyService _propertyService;

    [SetUp]
    public void Setup()
    {
        _mockRepository = new Mock<IPropertyRepository>();
        _mockMapper = new Mock<IMapper>();
        _propertyService = new PropertyService(_mockRepository.Object, _mockMapper.Object);
    }

    [Test]
    public async Task GetAllPropertiesAsync_ShouldReturnMappedProperties()
    {
        // Arrange
        var properties = new List<PropertyEntity>
        {
            new PropertyEntity { Id = "1", Name = "Test Property 1", Price = 100000 },
            new PropertyEntity { Id = "2", Name = "Test Property 2", Price = 200000 }
        };

        var propertyDtos = new List<PropertyDto>
        {
            new PropertyDto { IdProperty = "1", Name = "Test Property 1", Price = 100000 },
            new PropertyDto { IdProperty = "2", Name = "Test Property 2", Price = 200000 }
        };

        _mockRepository.Setup(r => r.GetAllWithDetailsAsync())
            .ReturnsAsync(properties);
        _mockMapper.Setup(m => m.Map<IEnumerable<PropertyDto>>(properties))
            .Returns(propertyDtos);

        // Act
        var result = await _propertyService.GetAllPropertiesAsync();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(2));
        _mockRepository.Verify(r => r.GetAllWithDetailsAsync(), Times.Once);
    }

    [Test]
    public async Task GetPropertyByIdAsync_WhenPropertyExists_ShouldReturnProperty()
    {
        // Arrange
        var propertyId = "1";
        var property = new PropertyEntity { Id = propertyId, Name = "Test Property", Price = 100000 };
        var propertyDto = new PropertyDto { IdProperty = propertyId, Name = "Test Property", Price = 100000 };

        _mockRepository.Setup(r => r.GetByIdWithDetailsAsync(propertyId))
            .ReturnsAsync(property);
        _mockMapper.Setup(m => m.Map<PropertyDto>(property))
            .Returns(propertyDto);

        // Act
        var result = await _propertyService.GetPropertyByIdAsync(propertyId);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.IdProperty, Is.EqualTo(propertyId));
        _mockRepository.Verify(r => r.GetByIdWithDetailsAsync(propertyId), Times.Once);
    }

    [Test]
    public async Task GetPropertyByIdAsync_WhenPropertyDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        var propertyId = "999";
        _mockRepository.Setup(r => r.GetByIdWithDetailsAsync(propertyId))
            .ReturnsAsync((PropertyEntity?)null);

        // Act
        var result = await _propertyService.GetPropertyByIdAsync(propertyId);

        // Assert
        Assert.That(result, Is.Null);
        _mockRepository.Verify(r => r.GetByIdWithDetailsAsync(propertyId), Times.Once);
    }

    [Test]
    public async Task GetFilteredPropertiesSimpleAsync_ShouldReturnSimpleProperties()
    {
        // Arrange
        var filter = new PropertyFilterDto
        {
            Name = "Test",
            MinPrice = 100000,
            MaxPrice = 500000
        };

        var properties = new List<PropertyEntity>
        {
            new PropertyEntity 
            { 
                Id = "1", 
                Name = "Test Property 1", 
                Price = 200000,
                IdOwner = "owner1",
                Address = "Test Address 1"
            }
        };

        _mockRepository.Setup(r => r.GetFilteredWithDetailsAsync(filter))
            .ReturnsAsync(properties);

        // Act
        var result = await _propertyService.GetFilteredPropertiesSimpleAsync(filter);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(1));
        var simpleProperty = result.First();
        Assert.That(simpleProperty.Name, Is.EqualTo("Test Property 1"));
        Assert.That(simpleProperty.Price, Is.EqualTo(200000));
        Assert.That(simpleProperty.IdOwner, Is.EqualTo("owner1"));
        Assert.That(simpleProperty.Address, Is.EqualTo("Test Address 1"));
    }

    [Test]
    public async Task CreatePropertyAsync_ShouldCreateAndReturnProperty()
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

        var property = new PropertyEntity
        {
            Id = "1",
            Name = createDto.Name,
            Address = createDto.Address,
            Price = createDto.Price,
            CodeInternal = createDto.CodeInternal,
            Year = createDto.Year,
            IdOwner = createDto.IdOwner
        };

        var propertyDto = new PropertyDto
        {
            IdProperty = "1",
            Name = createDto.Name,
            Address = createDto.Address,
            Price = createDto.Price,
            CodeInternal = createDto.CodeInternal,
            Year = createDto.Year,
            IdOwner = createDto.IdOwner
        };

        _mockMapper.Setup(m => m.Map<PropertyEntity>(createDto))
            .Returns(property);
        _mockRepository.Setup(r => r.CreateAsync(property))
            .ReturnsAsync(property);
        _mockRepository.Setup(r => r.GetByIdWithDetailsAsync(property.Id))
            .ReturnsAsync(property);
        _mockMapper.Setup(m => m.Map<PropertyDto>(property))
            .Returns(propertyDto);

        // Act
        var result = await _propertyService.CreatePropertyAsync(createDto);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Name, Is.EqualTo(createDto.Name));
        _mockRepository.Verify(r => r.CreateAsync(property), Times.Once);
    }

    [Test]
    public async Task UpdatePropertyAsync_WhenPropertyExists_ShouldUpdateProperty()
    {
        // Arrange
        var propertyId = "1";
        var updateDto = new UpdatePropertyDto
        {
            Name = "Updated Property",
            Price = 400000
        };

        var existingProperty = new PropertyEntity
        {
            Id = propertyId,
            Name = "Original Property",
            Price = 300000
        };

        _mockRepository.Setup(r => r.GetByIdAsync(propertyId))
            .ReturnsAsync(existingProperty);

        // Act
        await _propertyService.UpdatePropertyAsync(propertyId, updateDto);

        // Assert
        _mockRepository.Verify(r => r.GetByIdAsync(propertyId), Times.Once);
        _mockRepository.Verify(r => r.UpdateAsync(propertyId, existingProperty), Times.Once);
    }

    [Test]
    public async Task UpdatePropertyAsync_WhenPropertyDoesNotExist_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var propertyId = "999";
        var updateDto = new UpdatePropertyDto { Name = "Updated Property" };

        _mockRepository.Setup(r => r.GetByIdAsync(propertyId))
            .ReturnsAsync((PropertyEntity?)null);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _propertyService.UpdatePropertyAsync(propertyId, updateDto));
        
        Assert.That(ex.Message, Does.Contain("Property with Id 999 not found"));
    }

    [Test]
    public async Task DeletePropertyAsync_WhenPropertyExists_ShouldDeleteProperty()
    {
        // Arrange
        var propertyId = "1";
        var existingProperty = new PropertyEntity { Id = propertyId, Name = "Test Property" };

        _mockRepository.Setup(r => r.GetByIdAsync(propertyId))
            .ReturnsAsync(existingProperty);

        // Act
        await _propertyService.DeletePropertyAsync(propertyId);

        // Assert
        _mockRepository.Verify(r => r.GetByIdAsync(propertyId), Times.Once);
        _mockRepository.Verify(r => r.DeleteAsync(propertyId), Times.Once);
    }

    [Test]
    public async Task PropertyExistsAsync_WhenPropertyExists_ShouldReturnTrue()
    {
        // Arrange
        var propertyId = "1";
        var existingProperty = new PropertyEntity { Id = propertyId, Name = "Test Property" };

        _mockRepository.Setup(r => r.GetByIdAsync(propertyId))
            .ReturnsAsync(existingProperty);

        // Act
        var result = await _propertyService.PropertyExistsAsync(propertyId);

        // Assert
        Assert.That(result, Is.True);
        _mockRepository.Verify(r => r.GetByIdAsync(propertyId), Times.Once);
    }

    [Test]
    public async Task PropertyExistsAsync_WhenPropertyDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        var propertyId = "999";
        _mockRepository.Setup(r => r.GetByIdAsync(propertyId))
            .ReturnsAsync((PropertyEntity?)null);

        // Act
        var result = await _propertyService.PropertyExistsAsync(propertyId);

        // Assert
        Assert.That(result, Is.False);
        _mockRepository.Verify(r => r.GetByIdAsync(propertyId), Times.Once);
    }
}
