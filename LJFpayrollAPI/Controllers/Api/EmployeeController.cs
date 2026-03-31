using LJFpayrollAPI.Models.DTOs.Employee.Requests;
using LJFpayrollAPI.Models.DTOs.Employee.Responses;
using LJFpayrollAPI.Services.EmployeeServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LJFpayrollAPI.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController(IEmployeeService employeeService) : ControllerBase
    {
        private readonly IEmployeeService _employeeService = employeeService;

        [HttpPost]
        public async Task<ActionResult<EmployeeResponse>> AddEmployee(EmployeeRequest request)
        {

            var result = await _employeeService.AddEmployeeAsync(request);
            return Ok(result);

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeResponse>>> GetAllEmployees()
        {
            var result = await _employeeService.GetAllEmployeesAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeResponse>> GetEmployeeById(int id)
        {
            var result = await _employeeService.GetEmployeeByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("no/{employeeNumber}")]
        public async Task<ActionResult<EmployeeResponse>> GetEmployeeByNo(string employeeNumber)
        {
            var result = await _employeeService.GetByEmployeeNumberAsync(employeeNumber);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<EmployeeResponse>> UpdateEmployee(int id, EmployeeRequest request)
        {
            var result = await _employeeService.UpdateEmployeeAsync(id, request);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEmployee(int id)
        {
            var success = await _employeeService.DeleteEmployeeAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
