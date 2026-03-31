using LJFpayrollAPI.Data;
using LJFpayrollAPI.Models.DTOs.Employee.Requests;
using LJFpayrollAPI.Models.Entities;
using LJFpayrollAPI.Services.EmployeeServices;
using LJFpayrollAPI.Services.GenerateEmployeeNumberServices;
using Microsoft.EntityFrameworkCore;
using Moq;


namespace LJFpayrollAPITests.ServicesTests.EmployeeServicesTests
{
        public class EmployeeServiceTests
        {
            private readonly Moq.Mock<IGenerateEmployeeNumberService> _mockGen;
            private readonly ApplicationDbContext _context;
            private readonly EmployeeService _service;

            public EmployeeServiceTests()
            {
                // 1. Setup In-Memory Database
                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique name per test run
                    .Options;

                _context = new ApplicationDbContext(options);

                // 2. Setup Mock for ID Generator
                _mockGen = new Mock<IGenerateEmployeeNumberService>();

                // 3. Initialize Service
                _service = new EmployeeService(_context, _mockGen.Object);
            }

            [Fact]
            public async Task AddEmployeeAsync_ShouldSaveToDatabase_WithGeneratedNumber()
            {
                // Arrange
                var request = new EmployeeRequest
                {
                    FirstName = "Leonel",
                    LastName = "Felezario",
                    DateOfBirth = new DateOnly(1993, 8, 12),
                    DailyRate = 1500,
                    WorkingDays = "MWF"
                };

                string fakeId = "SYB-12345-12AUG1993";
                _mockGen.Setup(g => g.GenerateEmployeeNumber(request.LastName, request.DateOfBirth))
                        .Returns(fakeId);

                // Act
                var result = await _service.AddEmployeeAsync(request);

                // Assert
                var savedEmployee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == result.Id);
                Assert.NotNull(savedEmployee);
                Assert.Equal(fakeId, savedEmployee.EmployeeNumber);
                Assert.Equal("Leonel", savedEmployee.FirstName);
            }

            [Fact]
            public async Task GetEmployeeByIdAsync_ShouldReturnNull_WhenEmployeeDoesNotExist()
            {
                // Act
                var result = await _service.GetEmployeeByIdAsync(999);

                // Assert
                Assert.Null(result);
            }

            [Fact]
            public async Task UpdateEmployeeAsync_ShouldModifyExistingRecord()
            {
                // Arrange - Seed an employee
                var existing = new Employee
                {
                    FirstName = "Old",
                    LastName = "Name",
                    EmployeeNumber = "OLD-123",
                    DailyRate = 2000,
                    DateOfBirth = new DateOnly(1990, 1, 1),
                    WorkingDays = "MWF"
                };
                _context.Employees.Add(existing);
                await _context.SaveChangesAsync();

                var updateRequest = new EmployeeRequest
                {
                    FirstName = "New",
                    LastName = "Name",
                    DailyRate = 2000,
                    DateOfBirth = new DateOnly(1990, 1, 1),
                    WorkingDays = "TTHS"
                };

                // Act
                await _service.UpdateEmployeeAsync(existing.Id, updateRequest);

                // Assert
                var updated = await _context.Employees.FindAsync(existing.Id);
                Assert.Equal("New", updated!.FirstName);
                Assert.Equal(2000, updated.DailyRate);
                Assert.Equal("TTHS", updated.WorkingDays);
            }

            [Fact]
            public async Task DeleteEmployeeAsync_ShouldCallMarkAsDeleted()
            {
                // Arrange
                var employee = new Employee {
                    FirstName = "New",
                    LastName = "Name",
                    EmployeeNumber = "OLD-123",
                    DailyRate = 2000,
                    DateOfBirth = new DateOnly(1990, 1, 1),
                    WorkingDays = "TTHS"
                };
                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();

                // Act
                var result = await _service.DeleteEmployeeAsync(employee.Id);

                // Assert
                Assert.True(result);
                var deletedEmployee = await _context.Employees.FindAsync(employee.Id);
                // Assuming MarkAsDeleted sets a property like IsDeleted = true
                Assert.True(deletedEmployee!.IsDeleted);
            }
        }
    }
