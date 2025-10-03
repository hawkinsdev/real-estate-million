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

        _mockRepository.Setup(r => r.GetAllAsync())
            .ReturnsAsync(properties);
        _mockMapper.Setup(m => m.Map<IEnumerable<PropertyDto>>(properties))
            .Returns(propertyDtos);

        // Act
        var result = await _propertyService.GetAllPropertiesAsync();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(2));
        _mockRepository.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Test]
    public async Task GetPropertyByIdAsync_WhenPropertyExists_ShouldReturnProperty()
    {
        // Arrange
        var propertyId = "1";
        var property = new PropertyEntity { Id = propertyId, Name = "Test Property", Price = 100000 };
        var propertyDto = new PropertyDto { IdProperty = propertyId, Name = "Test Property", Price = 100000 };

        _mockRepository.Setup(r => r.GetByIdAsync(propertyId))
            .ReturnsAsync(property);
        _mockMapper.Setup(m => m.Map<PropertyDto>(property))
            .Returns(propertyDto);

        // Act
        var result = await _propertyService.GetPropertyByIdAsync(propertyId);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.IdProperty, Is.EqualTo(propertyId));
        _mockRepository.Verify(r => r.GetByIdAsync(propertyId), Times.Once);
    }

    [Test]
    public async Task GetPropertyByIdAsync_WhenPropertyDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        var propertyId = "999";
        _mockRepository.Setup(r => r.GetByIdAsync(propertyId))
            .ReturnsAsync((PropertyEntity?)null);

        // Act
        var result = await _propertyService.GetPropertyByIdAsync(propertyId);

        // Assert
        Assert.That(result, Is.Null);
        _mockRepository.Verify(r => r.GetByIdAsync(propertyId), Times.Once);
    }






}
