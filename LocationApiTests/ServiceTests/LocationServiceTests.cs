using LocationLibrary.Abstractions;
using LocationLibrary.Contracts.Dtos;
using LocationLibrary.Contracts.Models;
using LocationLibrary.Repos;
using LocationLibrary.Services;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace LocationApiTests.ServiceTests
{
    [TestClass]
    public class LocationServiceTests
    {
        private Mock<ICache> _mockCache;
        private Mock<IRepo<Location>> _mockLocationRepo;
        private IServiceProvider _serviceProvider;

        [TestMethod]
        public async Task AddLocation_GivenNameAndAddress_WhenCalled_ThenCreatesLocation()
        {
            // Arrange
            var testName = "test-name";
            var testAddress = "test-address";
            var testId = Guid.NewGuid().ToString();

            var expectedDto = new LocationDto
            { 
                Id = testId,
                Name = testName, 
                Address = testAddress 
            };

            var testAddedLocation = new Location
            {
                Id = testId,
                Name = testName,
                Address = testAddress
            };

            _mockLocationRepo.Setup(r => r.Create(It.IsAny<Location>()))
                .ReturnsAsync(testAddedLocation);

            var sut = _serviceProvider.GetRequiredService<LocationService>();
            // Act
            var testResult = await sut.AddLocation(testName, testAddress);

            // Assert
            _mockLocationRepo.Verify(r => r.Create(It.Is<Location>(m => m.Name.Equals(testName) && m.Address.Equals(testAddress))), Times.Once());
            _mockCache.Verify(c => c.Set(testId, testAddedLocation), Times.Once());

            Assert.AreEqual(expectedDto.Id, testResult.Id);
            Assert.AreEqual(expectedDto.Name, testResult.Name);
            Assert.AreEqual(expectedDto.Address, testResult.Address);
        }

        [TestMethod]
        public async Task DeleteLocation_GivenId_WhenCalled_GetsLocationsFromRepo()
        {
            // Arrange
            var testId = Guid.NewGuid().ToString();

            var sut = _serviceProvider.GetRequiredService<LocationService> ();

            // Act
            await sut.DeleteLocation(testId);

            // Assert
            _mockLocationRepo.Verify(l => l.Delete(testId), Times.Once());
            _mockCache.Verify(c => c.Remove(testId), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task DeleteLocation_GivenNullId_WhenCalled_ThrowsError()
        {
            // Arrange
            var sut = _serviceProvider.GetRequiredService<LocationService>();

            // Act
            await sut.DeleteLocation(null);

            // Assert
            _mockLocationRepo.Verify(l => l.Delete(It.IsAny<string>()), Times.Never());
            _mockCache.Verify(c => c.Remove(It.IsAny<string>()), Times.Never());
        }

        [TestMethod]
        public async Task GetAllLocations_GivenLocationCache_WhenCalled_RetrievesFromCache()
        {
            // Arrange
            IEnumerable<Location> cachedItems = new List<Location>()
            {
                new Location() { Id = Guid.NewGuid().ToString() },
                new Location() { Id = Guid.NewGuid().ToString() }
            };

            _mockCache.Setup(c => c.GetAll<Location>()).Returns(cachedItems);

            var sut = _serviceProvider.GetRequiredService<LocationService>();

            // Act
            var testResult = await sut.GetAllLocations();

            // Assert
            _mockCache.Verify(c => c.GetAll<Location>(), Times.Once());
            _mockLocationRepo.Verify(r => r.GetAll(), Times.Never());
        }

        [TestMethod]
        public async Task GetAllLocations_GivenEmptyCache_WhenCalled_RetrievesFromRepo()
        {
            // Arrange
            ICollection<Location> dataItems = new List<Location>()
            {
                new Location() { Id = Guid.NewGuid().ToString() },
                new Location() { Id = Guid.NewGuid().ToString() }
            };

            _mockLocationRepo.Setup(r => r.GetAll()).ReturnsAsync(dataItems);

            var sut = _serviceProvider.GetRequiredService<LocationService> ();

            // Act
            var testResult = await sut.GetAllLocations();

            // Assert
            _mockCache.Verify(c => c.GetAll<Location>(), Times.Once());
            _mockLocationRepo.Verify(r => r.GetAll(), Times.Once());
        }

        [TestMethod]
        public async Task GetAllLocations_GivenCacheWithEmptyList_RetrievesFromRepo()
        {
            // Arrange
            var cachedItems = new List<Location>();

            ICollection<Location> dataItems = new List<Location>()
            {
                new Location() { Id = Guid.NewGuid().ToString() },
                new Location() { Id = Guid.NewGuid().ToString() }
            };

            _mockCache.Setup(c => c.GetAll<Location>()).Returns(cachedItems);
            _mockLocationRepo.Setup(r => r.GetAll()).ReturnsAsync(dataItems);

            var sut = _serviceProvider.GetRequiredService<LocationService>();

            // Act
            var testResults = await sut.GetAllLocations();

            // Assert
            _mockCache.Verify(c => c.GetAll<Location>(), Times.Once());
            _mockLocationRepo.Verify(r => r.GetAll(), Times.Once());
        }

        [TestMethod]
        public async Task GetLocation_GivenLocationId_WhenCalled_ReturnsLocation()
        {
            // Arrange
            var testUpdatedName = "test-name-updated";
            var testUpdatedAddress = "test-name-updated";
            var testId = Guid.NewGuid().ToString();

            var testLocation = new Location
            {
                Id = testId,
                Name = testUpdatedName,
                Address = testUpdatedAddress
            };

            _mockCache.Setup(c => c.Get<Location>(It.IsAny<string>())).Returns(null as Location);
            _mockLocationRepo.Setup(r => r.Get(It.IsAny<string>())).ReturnsAsync(testLocation);

            var sut = _serviceProvider.GetRequiredService<LocationService>();

            // Act
            var testResult = await sut.GetLocation(testId);

            // Assert
            _mockCache.Verify(c => c.Get<Location>(testId), Times.Once());
            _mockLocationRepo.Verify(r => r.Get(testId), Times.Once());
        }

        [TestMethod]
        public async Task GetLocation_GivenLocationId_WhenCalled_ReturnsLocationFromCache()
        {
            // Arrange
            var testUpdatedName = "test-name-updated";
            var testUpdatedAddress = "test-name-updated";
            var testId = Guid.NewGuid().ToString();

            var testLocation = new Location
            {
                Id = testId,
                Name = testUpdatedName,
                Address = testUpdatedAddress
            };

            _mockCache.Setup(c => c.Get<Location>(It.IsAny<string>())).Returns(testLocation);

            var sut = _serviceProvider.GetRequiredService<LocationService>();

            // Act
            var testResult = await sut.GetLocation(testId);

            // Assert
            _mockCache.Verify(c => c.Get<Location>(testId), Times.Once());
            _mockLocationRepo.Verify(r => r.Get(testId), Times.Never());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task GetLocation_GivenNullLocationId_WhenCalled_ThrowsError()
        {
            // Arrange
            

            var sut = _serviceProvider.GetRequiredService<LocationService>();

            // Act
            var testResult = await sut.GetLocation(null);

            // Assert
            _mockCache.Verify(c => c.Get<Location>(It.IsAny<string>()), Times.Never());
            _mockLocationRepo.Verify(r => r.Get(It.IsAny<string>()), Times.Never());
        }

        [TestMethod]
        public async Task UpdateLocation_GivenLocationDto_WhenCalled_UpdatesLocation()
        {
            // Arrange
            var testUpdatedName = "test-name-updated";
            var testUpdatedAddress = "test-name-updated";

            var testId = Guid.NewGuid().ToString();

            var testDto = new LocationDto
            {
                Id = testId,
                Name = testUpdatedName,
                Address = testUpdatedAddress
            };

            var expectedUpdatedModel = new Location
            {
                Id = testId,
                Name = testUpdatedName,
                Address = testUpdatedAddress
            };

            _mockLocationRepo.Setup(r => r.Update(It.IsAny<Location>()))
                .ReturnsAsync (expectedUpdatedModel);

            var sut = _serviceProvider.GetRequiredService<LocationService>();

            // Act
            var testResult = await sut.UpdateLocation(testDto);

            // Assert
            _mockLocationRepo.Verify(
                r => r.Update(It.Is<Location>(l => l.Id.Equals(testId)
                    && l.Name.Equals(testUpdatedName)
                    && l.Address.Equals(testUpdatedAddress))), Times.Once());
            _mockCache.Verify(
                c => c.Set(testId, It.Is<Location>(l => l.Id.Equals(testId)
                    && l.Name.Equals(testUpdatedName)
                    && l.Address.Equals(testUpdatedAddress))), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task UpdateLocation_GivenNullLocationDto_WhenCalled_ThrowsException()
        {
            // Arrange

            var sut = _serviceProvider.GetRequiredService<LocationService>();

            // Act
            var testResult = await sut.UpdateLocation(null);

            // Assert
            _mockLocationRepo.Verify(r => r.Update(It.IsAny<Location>()), Times.Never());
            _mockCache.Verify(c => c.Set(It.IsAny<string>(), It.IsAny<Location>()), Times.Never());
        }

        [TestInitialize]
        public void Setup()
        {
            _mockCache = new Mock<ICache>();

            _mockLocationRepo = new Mock<IRepo<Location>>();

            var services =  new ServiceCollection();
            _serviceProvider = services.AddSingleton<ICache>(_mockCache.Object)
                .AddSingleton<IRepo<Location>>(_mockLocationRepo.Object)
                .AddTransient<LocationService>()
                .BuildServiceProvider();
        }
    }
}
