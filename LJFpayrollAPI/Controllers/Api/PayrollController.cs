using LJFpayrollAPI.Services.EmployeeServices;
using LJFpayrollAPI.Services.PayrollServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LJFpayrollAPI.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayrollController(IEmployeeService employeeService, IPayrollService payrollService) : ControllerBase
    {

        private readonly IEmployeeService _employeeService = employeeService;
        private readonly IPayrollService _payrollService = payrollService;

        [HttpGet("compute/{id}")]
        public async Task<IActionResult> Compute(string id, DateOnly start, DateOnly end)
        {
            var employee = await _employeeService.GetEntityByEmployeeNumberAsync(id);
            if (employee == null) return NotFound();

            var totalAmount = _payrollService.CalculatePay(employee, start, end);

            return Ok(totalAmount);
        }
    }
}
