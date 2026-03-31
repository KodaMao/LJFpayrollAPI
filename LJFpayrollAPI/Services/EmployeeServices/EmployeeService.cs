using LJFpayrollAPI.Data;
using LJFpayrollAPI.Mappings;
using LJFpayrollAPI.Models.DTOs.Employee.Requests;
using LJFpayrollAPI.Models.DTOs.Employee.Responses;
using LJFpayrollAPI.Models.Entities;
using LJFpayrollAPI.Services.GenerateEmployeeNumberServices;
using Microsoft.EntityFrameworkCore;

namespace LJFpayrollAPI.Services.EmployeeServices
{
    public class EmployeeService(ApplicationDbContext context, IGenerateEmployeeNumberService gen) : IEmployeeService
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IGenerateEmployeeNumberService _gen = gen;

        public async Task<IEnumerable<EmployeeResponse>> GetAllEmployeesAsync()
        {
            var employees = await _context.Employees.ToListAsync();
            return employees.Select(e => e.ToResponse()!);
        }

        public async Task<EmployeeResponse?> GetEmployeeByIdAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            return employee?.ToResponse();
        }

        public async Task<EmployeeResponse?> GetByEmployeeNumberAsync(string employeeNumber)
        {
            // We use FirstOrDefaultAsync because EmployeeNumber is Unique
            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.EmployeeNumber == employeeNumber);

            return employee?.ToResponse();
        }

        public async Task<EmployeeResponse> AddEmployeeAsync(EmployeeRequest request)
        {
            string generatedEmployeeNo = _gen.GenerateEmployeeNumber(request.LastName, request.DateOfBirth);

            var entity = request.ToEntity(generatedEmployeeNo);
            _context.Employees.Add(entity);
            await _context.SaveChangesAsync();

            return entity.ToResponse()!;
        }

        public async Task<EmployeeResponse?> UpdateEmployeeAsync(int id, EmployeeRequest request)
        {
            var existing = await _context.Employees.FindAsync(id);
            if (existing == null) return null;

            existing.FirstName = request.FirstName.Trim();
            existing.LastName = request.LastName.Trim();
            existing.MiddleName = request.MiddleName?.Trim();
            existing.DailyRate = request.DailyRate;
            existing.WorkingDays = request.WorkingDays.ToUpper();

            await _context.SaveChangesAsync();
            return existing.ToResponse();
        }

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            var existing = await _context.Employees.FindAsync(id);
            if (existing == null) return false;
            existing.MarkAsDeleted();
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Employee?> GetEntityByEmployeeNumberAsync(string employeeNumber)
        {
            return await _context.Employees
                .FirstOrDefaultAsync(e => e.EmployeeNumber == employeeNumber);
        }
    }
}
