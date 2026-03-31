using LJFpayrollAPI.Models.DTOs.Employee.Requests;
using LJFpayrollAPI.Models.DTOs.Employee.Responses;
using LJFpayrollAPI.Models.Entities;

namespace LJFpayrollAPI.Mappings
{
    public static class EmployeeMappingExtensions
    {
        public static EmployeeResponse? ToResponse(this Employee? employee)
        {
            if (employee == null) return null;

            return new EmployeeResponse
            {
                Id = employee.Id,
                EmployeeNumber = employee.EmployeeNumber,
                FullName = $"{employee.FirstName} {employee.LastName}",
                DateOfBirth = employee.DateOfBirth,
                DailyRate = employee.DailyRate,
                WorkingDays = employee.WorkingDays
            };
        }

        public static Employee ToEntity(this EmployeeRequest request, string generatedEmployeeNo)
        {

            var schedule = request.WorkingDays.Trim().ToUpper();

            // Validation 
            if (schedule != "MWF" && schedule != "TTHS")
            {
                throw new ArgumentException("Invalid work schedule. Accepted values are 'MWF' or 'TTHS'.");
            }

            return new Employee
            {
                EmployeeNumber = generatedEmployeeNo,
                FirstName = request.FirstName.Trim(),
                LastName = request.LastName.Trim(),
                MiddleName = request.MiddleName?.Trim(),
                DateOfBirth = request.DateOfBirth,
                DailyRate = request.DailyRate,
                WorkingDays = request.WorkingDays.ToUpper(),
                IsDeleted = false
            };
        }

        public static void UpdateEntity(this Employee employee, EmployeeRequest request)
        {
            var schedule = request.WorkingDays.Trim().ToUpper();
            // Validation 
            if (schedule != "MWF" && schedule != "TTHS")
            {
                throw new ArgumentException("Invalid work schedule. Accepted values are 'MWF' or 'TTHS'.");
            }
            employee.FirstName = request.FirstName.Trim();
            employee.LastName = request.LastName.Trim();
            employee.MiddleName = request.MiddleName?.Trim();
            employee.DateOfBirth = request.DateOfBirth;
            employee.DailyRate = request.DailyRate;
            employee.WorkingDays = request.WorkingDays.ToUpper();
        }

        public static void MarkAsDeleted(this Employee employee)
        {
            employee.IsDeleted = true;
        }
    }
}