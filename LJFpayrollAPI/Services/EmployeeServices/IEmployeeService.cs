using LJFpayrollAPI.Models.DTOs.Employee.Requests;
using LJFpayrollAPI.Models.DTOs.Employee.Responses;
using LJFpayrollAPI.Models.Entities;

namespace LJFpayrollAPI.Services.EmployeeServices
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeResponse>> GetAllEmployeesAsync();
        Task<EmployeeResponse?> GetEmployeeByIdAsync(int id);
        Task<EmployeeResponse?> GetByEmployeeNumberAsync(string employeeNumber);
        Task<EmployeeResponse> AddEmployeeAsync(EmployeeRequest request);
        Task<EmployeeResponse?> UpdateEmployeeAsync(int id, EmployeeRequest request);
        Task<bool> DeleteEmployeeAsync(int id);
        Task<Employee?> GetEntityByEmployeeNumberAsync(string employeeNumber);
    }
}
