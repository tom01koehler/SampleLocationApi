using LocationApi.Controllers;
using LocationLibrary.Contracts.Dtos;
using LocationLibrary.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Net;

namespace LocationApiTests.ControllerTests
{
    [TestClass]
    public class LocationControllerTests
    {
        private Mock<ILocationService> _mockLocationService;
        private LocationController _testController;

        [TestMethod]
        public async Task Create_GivenNameAddress_WhenCalled_ReturnsOk()
        {
            // Arrange
            var testName = "test-name";
            var testAddress = "test-address";

            // Act
            var testResult = await _testController.Create(testName, testAddress);

            // Assert
            var result = testResult.Result as ObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);
            _mockLocationService.Verify(l => l.AddLocation(testName, testAddress), Times.Once);
        }

        [TestMethod]
        public async Task Create_GivenNoName_WhenCalled_ReturnsBadRequest()
        {
            // Arrange
            var testAddress = "test-address";

            // Act
            var testResult = await _testController.Create(null, testAddress);

            // Assert
            var result = testResult.Result as ObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
            _mockLocationService.Verify(l => l.AddLocation(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task Create_GivenNullException_WhenCalled_ReturnsBadRequest()
        {
            // Arrange
            _mockLocationService.Setup(l => l.AddLocation(It.IsAny<string>(), It.IsAny<string>())).ThrowsAsync(new ArgumentNullException());

            // Act
            var testResult = await _testController.Create("test", "test");

            // Assert
            var result = testResult.Result as ObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public async Task Create_GivenArgumentException_WhenCalled_ReturnsConflict()
        {
            // Arrange
            _mockLocationService.Setup(l => l.AddLocation(It.IsAny<string>(), It.IsAny<string>())).ThrowsAsync(new ArgumentException());

            // Act
            var testResult = await _testController.Create("test", "test");

            // Assert
            var result = testResult.Result as ObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.Conflict, result.StatusCode);
        }

        [TestMethod]
        public async Task GetAll_WhenCalled_ReturnsOk()
        {
            // Arrange
            _mockLocationService.Setup(l => l.GetAllLocations()).ReturnsAsync(new List<LocationDto>());

            // Act
            var testResult = await _testController.GetAll();

            // Assert
            var result = testResult.Result as ObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);
            _mockLocationService.Verify(l => l.GetAllLocations(), Times.Once());
        }

        [TestMethod]
        public async Task Get_GivenId_WhenCalled_ReturnsOk()
        {
            // Arrange
            var testId = Guid.NewGuid().ToString();
            var testDto = new LocationDto
            {
                Id = testId,
                Name = "test-name",
                Address = "test-address"
            };

            _mockLocationService.Setup(l => l.GetLocation(It.IsAny<string>())).ReturnsAsync(testDto);

            // Act
            var testResult = await _testController.Get(testId);

            // Assert
            var result = testResult.Result as ObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);
            _mockLocationService.Verify(l => l.GetLocation(testId), Times.Once);
        }

        [TestMethod]
        public async Task Get_GivenNullId_WhenCalled_ReturnsBadRequest()
        { 
            // Arrange
            // Act
            var testResult = await _testController.Get(null);

            // Assert
            var result = testResult.Result as BadRequestResult;
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
            _mockLocationService.Verify(l => l.GetLocation(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task Get_GivenId_WhenServiceThrows_ReturnsBadRequest()
        {
            // Arrange
            var testId = Guid.NewGuid().ToString();

            _mockLocationService.Setup(l => l.GetLocation(It.IsAny<string>())).ThrowsAsync(new ArgumentNullException());

            // Act
            var testResult = await _testController.Get(testId);

            // Assert
            var result = testResult.Result as BadRequestObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
            _mockLocationService.Verify(l => l.GetLocation(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public async Task Get_GivenId_WhenServiceReturnsNull_ReturnsNotFound()
        {
            // Arrange
            var testId = Guid.NewGuid().ToString();

            _mockLocationService.Setup(l => l.GetLocation(It.IsAny<string>())).ReturnsAsync(null as LocationDto);

            // Act
            var testResult = await _testController.Get(testId);

            // Assert
            var result = testResult.Result as NotFoundObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.NotFound, result.StatusCode);
            _mockLocationService.Verify(l => l.GetLocation(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public async Task Put_GivenUpdate_WhenCalled_ReturnsOk()
        {
            // Arrange
            var testDto = new LocationDto
            {
                Id = Guid.NewGuid().ToString(),
                Name = "test-name",
                Address = "test-address"
            };

            // Act
            var testResult = await _testController.Put(testDto);

            // Assert
            var result = testResult.Result as ObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);
            _mockLocationService.Verify(l => l.UpdateLocation(testDto), Times.Once());
        }

        [TestMethod]
        [DataRow(null, "test-name", "test-address")]
        [DataRow("test-id", null, "test-address")]
        public async Task Put_GivenUpdate_WhenProperty_ReturnsBadRequest(string testId, string testName, string testAddress)
        {
            // Arrange
            var testDto = new LocationDto
            {
                Id = testId,
                Name = testName,
                Address = testAddress
            };

            // Act
            var testResult = await _testController.Put(testDto);

            // Assert
            var result = testResult.Result as ObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
            _mockLocationService.Verify(l => l.UpdateLocation(It.IsAny<LocationDto>()), Times.Never());
        }

        [TestMethod]
        public async Task Delete_GivenId_WhenCalled_ReturnsNoContent()
        {
            // Arrange
            var testId = Guid.NewGuid().ToString();

            _mockLocationService.Setup(l => l.DeleteLocation(It.IsAny<string>()));

            // Act
            var testResult = await _testController.Delete(testId);

            // Assert
            var result = testResult as NoContentResult;
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.NoContent, result.StatusCode);
            _mockLocationService.Verify(l => l.DeleteLocation(testId), Times.Once);
        }

        [TestMethod]
        public async Task Delete_Given_WhenCalledServiceThrowsArgumentNullException_ReturnsBadRequest()
        {
            // Arrange
            var testId = Guid.NewGuid().ToString();

            _mockLocationService.Setup(l => l.DeleteLocation(It.IsAny<string>())).ThrowsAsync(new ArgumentNullException());

            // Act
            var testResult = await _testController.Delete(testId);

            // Assert
            var result = testResult as BadRequestObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
            _mockLocationService.Verify(l => l.DeleteLocation(testId), Times.Once);
        }

        [TestMethod]
        public async Task Delete_Given_WhenCalledServiceThrowsKeyNotFoundExceiption_ReturnsBadRequest()
        {
            // Arrange
            var testId = Guid.NewGuid().ToString();

            _mockLocationService.Setup(l => l.DeleteLocation(It.IsAny<string>())).ThrowsAsync(new KeyNotFoundException());

            // Act
            var testResult = await _testController.Delete(testId);

            // Assert
            var result = testResult as NotFoundObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.NotFound, result.StatusCode);
            _mockLocationService.Verify(l => l.DeleteLocation(testId), Times.Once);
        }

        [TestMethod]
        public async Task Put_GivenUpdate_WhenLocationNotFound_ReturnsNotFound()
        {
            // Arrange
            var testDto = new LocationDto
            {
                Id = Guid.NewGuid().ToString(),
                Name = "test-name",
                Address = "test-address",
            };

            _mockLocationService.Setup(l => l.UpdateLocation(It.IsAny<LocationDto>())).ThrowsAsync(new KeyNotFoundException());

            // Act
            var testResult = await _testController.Put(testDto);

            // Assert
            var result = testResult.Result as NotFoundObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.NotFound, result.StatusCode);
        }

        [TestMethod]
        public async Task Put_GivenUpdate_WhenLocationUpdatedError_ReturnsConflict()
        {
            // Arrange
            var testDto = new LocationDto
            {
                Id = Guid.NewGuid().ToString(),
                Name = "test-name",
                Address = "test-address",
            };

            _mockLocationService.Setup(l => l.UpdateLocation(It.IsAny<LocationDto>())).ThrowsAsync(new DbUpdateException());

            // Act
            var testResult = await _testController.Put(testDto);

            // Assert
            var result = testResult.Result as ConflictObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.Conflict, result.StatusCode);
        }

        [TestMethod]
        public async Task Delete_GivenId_WhenNotFound_ReturnsNotFound()
        {
            // Arrange
            _mockLocationService.Setup(l => l.DeleteLocation(It.IsAny<string>())).ThrowsAsync(new KeyNotFoundException());

            // Act
            var testResult = await _testController.Delete("test-id");

            // Assert
            var result = testResult as NotFoundObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.NotFound, result.StatusCode);
        }

        [TestInitialize]
        public void Setup()
        {
            _mockLocationService = new Mock<ILocationService>();

            _testController = new LocationController(_mockLocationService.Object);
        }
    }
}
