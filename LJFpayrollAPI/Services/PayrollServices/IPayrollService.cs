using LJFpayrollAPI.Models.Entities;

namespace LJFpayrollAPI.Services.PayrollServices
{
    public interface IPayrollService
    {
         decimal CalculatePay(Employee employee, DateOnly startDate, DateOnly endDate);
    }
}
