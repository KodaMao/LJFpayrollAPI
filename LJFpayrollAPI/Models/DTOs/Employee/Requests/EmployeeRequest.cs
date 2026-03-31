namespace LJFpayrollAPI.Models.DTOs.Employee.Requests
{
    public class EmployeeRequest
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? MiddleName { get; set; }
        public required DateOnly DateOfBirth { get; set; }
        public required decimal DailyRate { get; set; }
        public required string WorkingDays { get; set; }
    }
}
