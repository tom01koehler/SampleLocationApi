using LocationLibrary.Contracts.Models;
using LocationLibrary.Data.DataContext;
using LocationLibrary.Repos;
using Microsoft.EntityFrameworkCore;
using Moq.EntityFrameworkCore;

namespace LocationApiTests.RepoTests
{
    [TestClass]
    public class LocationRepoTests
    {
        private Mock<LocationDbContext> _mockLocationContext;
        private LocationRepo _testRepo;

        private string _seededId = Guid.NewGuid().ToString();
        private string _seededName = "test-name-1";

        [TestMethod]
        public async Task Create_GivenData_WhenCalled_AddsData()
        {
            // Arrange
            var testLocation = new Location
            {
                Name = "test-name",
                Address = "test-address"
            };

            // Act
            await _testRepo.Create(testLocation);

            // Assert
            _mockLocationContext.Verify(c => c.AddAsync(It.IsAny<Location>(), It.IsAny<CancellationToken>()), Times.Once);
            _mockLocationContext.Verify(l => l.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task Create_GivenData_WhenNameIsDuplicate_ThrowsException()
        {
            // Arrange
            var testLocation = new Location
            {
                Name = _seededName,
                Address = "test-new-address"
            };

            // Act
            await _testRepo.Create(testLocation);

            // Assert
            _mockLocationContext.Verify(c => c.Locations.AddAsync(It.IsAny<Location>(), It.IsAny<CancellationToken>()), Times.Never);
            _mockLocationContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task Create_GivenData_WhenNameIsNull_ThrowsException()
        {
            // Arrange
            var testLocation = new Location
            {
                Address = "test-new-address"
            };

            // Act
            await _testRepo.Create(testLocation);

            // Assert
            _mockLocationContext.Verify(c => c.Locations.AddAsync(It.IsAny<Location>(), It.IsAny<CancellationToken>()), Times.Never);
            _mockLocationContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [TestMethod]
        public async Task Delete_GivenId_WhenCalled_DeletesRecord()
        {
            // Arange
            _mockLocationContext.Setup(c => c.FindAsync<Location>(It.IsAny<string>())).ReturnsAsync(new Location());

            // Act
            await _testRepo.Delete(_seededId);

            // Assert
            _mockLocationContext.Verify(c => c.FindAsync<Location>(_seededId), Times.Once);
            _mockLocationContext.Verify(c => c.Remove(It.IsAny<Location>()), Times.Once);
            _mockLocationContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public async Task Delete_GivenInvalidId_WhenCalled_ThrowsException()
        {
            // Arrange
            var invalidId = Guid.NewGuid().ToString();
            _mockLocationContext.Setup(c => c.FindAsync<Location>(It.IsAny<string>())).ReturnsAsync(null as Location);

            // Act
            await _testRepo.Delete(invalidId);

            // Assert
            _mockLocationContext.Verify(c => c.FindAsync<Location>(_seededId), Times.Once);
            _mockLocationContext.Verify(c => c.Remove(It.IsAny<Location>()), Times.Never);
            _mockLocationContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task Delete_GivenNullId_WhenCalled_ThrowsException()
        {
            // Arrange
            _mockLocationContext.Setup(c => c.Locations.FindAsync(It.IsAny<string>())).ReturnsAsync(null as Location);

            // Act
            await _testRepo.Delete(null);

            // Assert
            _mockLocationContext.Verify(c => c.FindAsync<Location>(_seededId), Times.Never);
            _mockLocationContext.Verify(c => c.Remove(It.IsAny<Location>()), Times.Never);
            _mockLocationContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [TestMethod]
        public async Task Get_GivenId_WhenCalled_ReturnsLocation()
        {
            // Arrange
            _mockLocationContext.Setup(c => c.FindAsync<Location>(It.IsAny<string>())).ReturnsAsync(new Location());

            // Act
            await _testRepo.Get(_seededId);

            // Assert
            _mockLocationContext.Verify(c => c.FindAsync<Location>(_seededId), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task Get_GivenNullId_WhenCalled_ThrowsException()
        {
            // Arrange
            // Act
            // Assert
            await _testRepo.Get(null);
        }

        [TestMethod]
        public async Task GetAll_GivenSeededData_WhenCalled_ReturnsAllData()
        {
            // Arrange
            // Act
            var testResult = await _testRepo.GetAll();

            // Assert
            Assert.IsTrue(testResult.Count > 0);
        }

        [TestMethod]
        public async Task Update_GivenUpdateData_WhenCalled_UpdatesData()
        {
            // Arrange
            var testUpdate = new Location
            {
                Id = _seededId,
                Name = "test-updated-name",
                Address = "test-updated-address"
            };

            _mockLocationContext.Setup(c => c.FindAsync<Location>(It.IsAny<string>())).ReturnsAsync(new Location());

            // Act
            await _testRepo.Update(testUpdate);

            // Assert
            _mockLocationContext.Verify(c => c.FindAsync<Location>(It.IsAny<string>()), Times.Once);
            _mockLocationContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public async Task Update_GivenInvalidKey_WhenCalled_ThrowsException()
        {
            // Arrange
            var invalidId = Guid.NewGuid().ToString();

            var testUpdate = new Location
            {
                Id = invalidId,
                Address = "test-updated-address",
                Name = "test-updated-name"
            };

            // Act
            await _testRepo.Update(testUpdate);

            // Assert
            _mockLocationContext.Verify(c => c.FindAsync<Location>(It.IsAny<string>()), Times.Once);
            _mockLocationContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);

        }

        [TestMethod]
        [ExpectedException(typeof(DbUpdateException))]
        public async Task Update_GivenDuplicateName_WhenCalled_ThrowsException()
        {
            // Arrange
            var testUpdate = new Location
            {
                Id = _seededId,
                Name = _seededName,
                Address = "test-updated-address"
            };

            _mockLocationContext.Setup(c => c.FindAsync<Location>(It.IsAny<string>())).ReturnsAsync(new Location());

            // Act
            await _testRepo.Update(testUpdate);

            // Assert
            _mockLocationContext.Verify(c => c.FindAsync<Location>(It.IsAny<string>()), Times.Once);
            _mockLocationContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task Update_GivenNullName_WhenCalled_ThrowsException()
        {
            // Arrange
            var testUpdate = new Location
            {
                Id = _seededId,
                Name = null,
                Address = "test-updated-address"
            };

            // Act
            await _testRepo.Update(testUpdate);

            // Assert
            _mockLocationContext.Verify(c => c.FindAsync<Location>(It.IsAny<string>()), Times.Once);
            _mockLocationContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [TestInitialize]
        public void Setup()
        {
            _mockLocationContext = new Mock<LocationDbContext>();

            var testLocations = new List<Location>()
            {
                new Location
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = _seededName,
                        Address = "test-address-1"
                    },
                    new Location
                    {
                        Id= _seededId,
                        Name = "test-name-2",
                        Address = "test-address-2"
                    },
                    new Location
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "test-name-3",
                        Address = "test-address-3"
                    }
            };

            _mockLocationContext.Setup(c => c.Locations).ReturnsDbSet(testLocations);
            _mockLocationContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            
            _testRepo = new LocationRepo(_mockLocationContext.Object);
        }
    }
}
