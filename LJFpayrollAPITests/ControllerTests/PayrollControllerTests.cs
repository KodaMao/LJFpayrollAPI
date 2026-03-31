using LJFpayrollAPI.Controllers.Api;
using LJFpayrollAPI.Models.Entities;
using LJFpayrollAPI.Services.EmployeeServices;
using LJFpayrollAPI.Services.PayrollServices;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LJFpayrollAPITests.ControllerTests
{
    public class PayrollControllerTests
    {
        private readonly Mock<IEmployeeService> _mockEmployeeService;
        private readonly Mock<IPayrollService> _mockPayrollService;
        private readonly PayrollController _controller;

        public PayrollControllerTests()
        {
            _mockEmployeeService = new Mock<IEmployeeService>();
            _mockPayrollService = new Mock<IPayrollService>();
            _controller = new PayrollController(_mockEmployeeService.Object, _mockPayrollService.Object);
        }

        [Fact]
        public async Task Compute_ShouldReturnOk_WhenEmployeeExists()
        {
            // Arrange
            string empNo = "SYB-12345";
            var startDate = new DateOnly(2026, 3, 1);
            var endDate = new DateOnly(2026, 3, 31);

            var fakeEmployee = new Employee
            {
                FirstName = "Leonel",
                LastName = "Sybico",
                EmployeeNumber = empNo,
                DailyRate = 1000,
                WorkingDays = "MWF",
                DateOfBirth = new DateOnly(1993, 8, 12)
            };

            _mockEmployeeService.Setup(s => s.GetEntityByEmployeeNumberAsync(empNo))
                                .ReturnsAsync(fakeEmployee);

            _mockPayrollService.Setup(s => s.CalculatePay(fakeEmployee, startDate, endDate))
                               .Returns(5000m); // Mocked calculation result

            // Act
            var result = await _controller.Compute(empNo, startDate, endDate);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(5000m, okResult.Value);

            // Verify the services were actually called
            _mockEmployeeService.Verify(s => s.GetEntityByEmployeeNumberAsync(empNo), Times.Once);
            _mockPayrollService.Verify(s => s.CalculatePay(fakeEmployee, startDate, endDate), Times.Once);
        }

        [Fact]
        public async Task Compute_ShouldReturnNotFound_WhenEmployeeDoesNotExist()
        {
            // Arrange
            string invalidNo = "NONE-000";
            _mockEmployeeService.Setup(s => s.GetEntityByEmployeeNumberAsync(invalidNo))
                                .ReturnsAsync((Employee?)null);

            // Act
            var result = await _controller.Compute(invalidNo, new DateOnly(), new DateOnly());

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
