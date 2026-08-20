using Moq;
using StayEasy.Application.DTOs.Hotels;
using StayEasy.Application.Interfaces.Repositories;
using StayEasy.Application.Services;
using StayEasy.Domain.Entities;

namespace StayEasy.Test;

    [TestFixture]
    public class HotelServiceTests
    {
        private Mock<IHotelRepository> _mockHotelRepo;
        private Mock<IRoomRepository> _mockRoomRepo;
        private HotelService _hotelService;
        [SetUp]
        public void Setup()
        {
            _mockHotelRepo = new Mock<IHotelRepository>();
            _mockRoomRepo = new Mock<IRoomRepository>();
            _hotelService = new HotelService(_mockHotelRepo.Object, _mockRoomRepo.Object);
        }
        [Test]
        public async Task GetHotelByIdAsync_ShouldThrowKeyNotFound_WhenHotelDoesNotExist()
        {
            // Arrange
            var hotelId = Guid.NewGuid();
            _mockHotelRepo.Setup(repo => repo.GetByIdWithRoomAsync(hotelId)).ReturnsAsync((Hotel)null);
            // Act & Assert
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () =>
                await _hotelService.GetHotelByIdAsync(hotelId));

            Assert.That(ex.Message, Is.EqualTo("Hotel not found."));
        }
        [Test]
        public async Task CreateHotelAsync_ShouldSaveToDatabase_AndReturnDto()
        {
            // Arrange
            var dto = new CreateHotelDto("Taj Mahal Palace", "Gateway Of India", "Mumbai");
            // Act
            var result = await _hotelService.CreateHotelAsync(dto);
            // Assert
            Assert.That(result.Name, Is.EqualTo("Taj Mahal Palace"));
            Assert.That(result.City, Is.EqualTo("Mumbai"));

            // Verify it was added to the DB
            _mockHotelRepo.Verify(repo => repo.AddAsync(It.IsAny<Hotel>()), Times.Once);
            _mockHotelRepo.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }
    }

