using Microsoft.AspNetCore.Mvc;
using Moq;
using StayEasy.Api.Controller;
using StayEasy.Application.DTOs.Hotels;
using StayEasy.Application.Services;

namespace StayEasy.Test;



    [TestFixture]
    public class HotelControllerTests
    {
        private Mock<IHotelService> _mockHotelService;
        private HotelController _hotelController;
        [SetUp]
        public void Setup()
        {
            _mockHotelService = new Mock<IHotelService>();
            _hotelController = new HotelController(_mockHotelService.Object);
        }
        [Test]
        public async Task GetHotelById_ShouldReturnOk_WithData()
        {
            // Arrange
            var hotelId = Guid.NewGuid();
            var expectedHotel = new HotelResponseDto(hotelId, "Test Hotel", "123 St", "TestCity", null);

            _mockHotelService.Setup(s => s.GetHotelByIdAsync(hotelId)).ReturnsAsync(expectedHotel);
            // Act
            var result = await _hotelController.GetHotelById(hotelId);
            // Assert
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo(expectedHotel));
        }
        [Test]
        public async Task AddHotel_ShouldReturnCreated_WhenValid()
        {
            // Arrange
            var dto = new CreateHotelDto("New Hotel", "Address", "City");
            var createdResponse = new HotelResponseDto(Guid.NewGuid(), "New Hotel", "Address", "City", null);
            _mockHotelService.Setup(s => s.CreateHotelAsync(dto)).ReturnsAsync(createdResponse);
            // Act
            var result = await _hotelController.AddHotel(dto);
            // Assert
            var createdResult = result as CreatedResult;
            Assert.That(createdResult, Is.Not.Null);
            Assert.That(createdResult.StatusCode, Is.EqualTo(201));
        }
    }

