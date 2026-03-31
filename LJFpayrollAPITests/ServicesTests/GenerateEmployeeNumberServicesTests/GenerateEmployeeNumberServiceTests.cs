using LJFpayrollAPI.Services.GenerateEmployeeNumberServices;

namespace LJFpayrollAPITests.ServicesTests.GenerateEmployeeNumberServicesTests
{
    public class GenerateEmployeeNumberServiceTests
    {
        private readonly GenerateEmployeeNumberService _service;

        public GenerateEmployeeNumberServiceTests()
        {
            _service = new GenerateEmployeeNumberService();
        }

        [Fact]
        public void GenerateEmployeeNumber_ShouldUseFirstThreeLetters_WhenNameIsLong()
        {
            // Arrange
            string lastName = "Sybico";
            var dob = new DateOnly(1993, 8, 12);

            // Act
            string result = _service.GenerateEmployeeNumber(lastName, dob);

            // Assert
            // Expected format: SYB-XXXXX-12AUG1993
            Assert.StartsWith("SYB-", result);
            Assert.EndsWith("-12AUG1993", result);
            Assert.Equal(19, result.Length); // 3 (prefix) + 1 (dash) + 5 (random) + 1 (dash) + 10 (date)
        }

        [Fact]
        public void GenerateEmployeeNumber_ShouldPadWithAsterisks_WhenNameIsShort()
        {
            // Arrange
            string lastName = "Li"; // Only 2 letters
            var dob = new DateOnly(2000, 1, 1);

            // Act
            string result = _service.GenerateEmployeeNumber(lastName, dob);

            // Assert
            // Expected prefix: LI*
            Assert.StartsWith("LI*-", result);
        }

        [Fact]
        public void GenerateEmployeeNumber_ShouldRemoveSpaces_BeforeGeneratingPrefix()
        {
            // Arrange
            string lastName = "De La Cruz";
            var dob = new DateOnly(1995, 12, 25);

            // Act
            string result = _service.GenerateEmployeeNumber(lastName, dob);

            // Assert
            // Should ignore spaces and take "DEL"
            Assert.StartsWith("DEL-", result);
        }

        [Theory]
        [InlineData(1, "JAN")]
        [InlineData(5, "MAY")]
        [InlineData(12, "DEC")]
        public void GenerateEmployeeNumber_ShouldFormatMonthCorrectly(int month, string expectedMonthStr)
        {
            // Arrange
            string lastName = "Test";
            var dob = new DateOnly(2020, month, 15);

            // Act
            string result = _service.GenerateEmployeeNumber(lastName, dob);

            // Assert
            Assert.Contains(expectedMonthStr, result);
        }
    }
}
