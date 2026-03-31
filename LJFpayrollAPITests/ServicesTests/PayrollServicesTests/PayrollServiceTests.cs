using LJFpayrollAPI.Models.Entities;
using LJFpayrollAPI.Services.PayrollServices;

namespace LJFpayrollAPITests.ServicesTests.PayrollServicesTests
{
    public class PayrollServiceTests
    {
        private readonly PayrollService _service;

        public PayrollServiceTests()
        {
            _service = new PayrollService();
        }

        [Fact]
        public void CalculatePay_ShouldReturnDoublePay_OnScheduledWorkDay()
        {
            // Arrange: Monday (MWF)
            var employee = CreateBaseEmployee("MWF");
            var start = new DateOnly(2026, 3, 30); // Monday
            var end = new DateOnly(2026, 3, 30);

            // Act
            var result = _service.CalculatePay(employee, start, end);

            // Assert: 1000 * 2 = 2000
            Assert.Equal(2000, result);
        }

        [Fact]
        public void CalculatePay_ShouldReturnZero_OnNonScheduledDay()
        {
            // Arrange: Tuesday (MWF)
            var employee = CreateBaseEmployee("MWF");
            var start = new DateOnly(2026, 3, 31); // Tuesday
            var end = new DateOnly(2026, 3, 31);

            // Act
            var result = _service.CalculatePay(employee, start, end);

            // Assert: 0
            Assert.Equal(0, result);
        }

        [Fact]
        public void CalculatePay_ShouldOnlyGiveBirthdayBonus_OnNonWorkDay()
        {
            // Arrange: Tuesday is Birthday (MWF Schedule)
            var employee = CreateBaseEmployee("MWF");
            employee.DateOfBirth = new DateOnly(1993, 3, 31); // Birthday is March 31

            var start = new DateOnly(2026, 3, 31); // Tuesday
            var end = new DateOnly(2026, 3, 31);

            // Act
            var result = _service.CalculatePay(employee, start, end);

            // Assert: 0 (not a work day) + 1000 (birthday bonus) = 1000
            Assert.Equal(1000, result);
        }

        [Fact]
        public void CalculatePay_ShouldTriplePay_WhenWorkDayIsBirthday()
        {
            // Arrange: Monday is Workday AND Birthday (MWF Schedule)
            var employee = CreateBaseEmployee("MWF");
            employee.DateOfBirth = new DateOnly(1993, 3, 30); // Birthday is March 30

            var start = new DateOnly(2026, 3, 30); // Monday
            var end = new DateOnly(2026, 3, 30);

            // Act
            var result = _service.CalculatePay(employee, start, end);

            // Assert: (1000 * 2) + 1000 = 3000
            Assert.Equal(3000, result);
        }

        [Theory]
        [InlineData("MWF", 6000)] // Mon, Wed, Fri (3 days * 2000)
        [InlineData("TTHS", 6000)] // Tue, Thu, Sat (3 days * 2000)
        public void CalculatePay_ShouldCalculateFullWeekCorrectly(string schedule, decimal expected)
        {
            // Arrange
            var employee = CreateBaseEmployee(schedule);
            var start = new DateOnly(2026, 3, 23); // Monday
            var end = new DateOnly(2026, 3, 29);   // Sunday

            // Act
            var result = _service.CalculatePay(employee, start, end);

            // Assert
            Assert.Equal(expected, result);
        }

        // Helper to keep tests clean and fix the "Required member" error
        private Employee CreateBaseEmployee(string schedule)
        {
            return new Employee
            {
                FirstName = "Test",
                LastName = "User",
                EmployeeNumber = "TST-12345",
                DailyRate = 1000,
                WorkingDays = schedule,
                DateOfBirth = new DateOnly(1990, 1, 1) // Neutral birthday
            };
        }
    }
}