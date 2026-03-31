using LJFpayrollAPI.Controllers.Api;
using LJFpayrollAPI.Models.DTOs.Employee.Requests;
using LJFpayrollAPI.Models.DTOs.Employee.Responses;
using LJFpayrollAPI.Services.EmployeeServices;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LJFpayrollAPITests.ControllerTests
{
    public class EmployeeControllerTests
    {
        private readonly Mock<IEmployeeService> _mockService;
        private readonly EmployeeController _controller;

        public EmployeeControllerTests()
        {
            _mockService = new Mock<IEmployeeService>();
            _controller = new EmployeeController(_mockService.Object);
        }

        [Fact]
        public async Task GetEmployeeById_ShouldReturnOk_WhenEmployeeExists()
        {
            // Arrange
            int testId = 1;
            var fakeResponse = new EmployeeResponse
            {
                Id = testId,
                FullName = "Felezario, Leonel",
                EmployeeNumber = "SYB-12345-12AUG1993",
                DateOfBirth = new DateOnly(1993, 8, 12),
                DailyRate = 1500,
                WorkingDays = "MWF"

            };

            _mockService.Setup(s => s.GetEmployeeByIdAsync(testId))
                        .ReturnsAsync(fakeResponse);

            // Act
            var result = await _controller.GetEmployeeById(testId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedValue = Assert.IsType<EmployeeResponse>(okResult.Value);
            Assert.Equal("Felezario, Leonel", returnedValue.FullName);
        }

        [Fact]
        public async Task GetEmployeeById_ShouldReturnNotFound_WhenEmployeeDoesNotExist()
        {
            // Arrange
            _mockService.Setup(s => s.GetEmployeeByIdAsync(999))
                        .ReturnsAsync((EmployeeResponse?)null);

            // Act
            var result = await _controller.GetEmployeeById(999);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task AddEmployee_ShouldReturnOk_WithCreatedData()
        {
            // Arrange
            var request = new EmployeeRequest {
                FirstName = "Leonel",
                LastName = "Felezario",
                DateOfBirth = new DateOnly(1993, 8, 12),
                DailyRate = 1500,
                WorkingDays = "MWF"
            };
            var response = new EmployeeResponse { Id = 1,
                EmployeeNumber = "OLD-123",
                FullName = "Felezario, Leonel",
                DateOfBirth = new DateOnly(1993, 8, 12),
                DailyRate = 1500,
                WorkingDays = "MWF"

            };

            _mockService.Setup(s => s.AddEmployeeAsync(request))
                        .ReturnsAsync(response);

            // Act
            var result = await _controller.AddEmployee(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(response, okResult.Value);
        }

        [Fact]
        public async Task DeleteEmployee_ShouldReturnNoContent_OnSuccess()
        {
            // Arrange
            int testId = 1;
            _mockService.Setup(s => s.DeleteEmployeeAsync(testId))
                        .ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteEmployee(testId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteEmployee_ShouldReturnNotFound_WhenIdIsInvalid()
        {
            // Arrange
            _mockService.Setup(s => s.DeleteEmployeeAsync(999))
                        .ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteEmployee(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
