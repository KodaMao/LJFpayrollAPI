namespace LJFpayrollAPI.Models.DTOs.Employee.Responses
{
    public class EmployeeResponse
    {
        public int Id { get; set; }
        public required string EmployeeNumber { get; set; }
        public required string FullName { get; set; }
        public required DateOnly DateOfBirth { get; set; }
        public required decimal DailyRate { get; set; }
        public required string WorkingDays { get; set; }
    }
}
