namespace LJFpayrollAPI.Models.Entities
{
    public class Employee
    {
        public int Id { get; set; }

        public required string EmployeeNumber { get; set; } 

        public required string FirstName { get; set; }

        public required string LastName { get; set; }

        public string? MiddleName { get; set; }

        public required DateOnly DateOfBirth { get; set; }

        public required decimal DailyRate { get; set; }

        public required string WorkingDays { get; set; } 

        public bool IsDeleted { get; set; } = false; 
    }
}
