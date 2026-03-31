using LJFpayrollAPI.Models.Entities;

namespace LJFpayrollAPI.Services.PayrollServices
{
    public class PayrollService : IPayrollService
    {
        public decimal CalculatePay(Employee employee, DateOnly startDate, DateOnly endDate)
        {
            decimal totalPay = 0;
            var current = startDate;

            while (current <= endDate)
            {
                if (IsScheduledDay(current, employee.WorkingDays))
                {
                    totalPay += (employee.DailyRate * 2);
                }

                if (current.Month == employee.DateOfBirth.Month && current.Day == employee.DateOfBirth.Day)
                {
                    totalPay += employee.DailyRate;
                }

                current = current.AddDays(1);
            }

            return totalPay;
        }

        private bool IsScheduledDay(DateOnly date, string schedule)
        {
            var day = date.DayOfWeek;
            return schedule switch
            {
                "MWF" => day == DayOfWeek.Monday || day == DayOfWeek.Wednesday || day == DayOfWeek.Friday,
                "TTHS" => day == DayOfWeek.Tuesday || day == DayOfWeek.Thursday || day == DayOfWeek.Saturday,
                _ => false
            };
        }
    }
}
