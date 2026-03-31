namespace LJFpayrollAPI.Services.GenerateEmployeeNumberServices
{
    public class GenerateEmployeeNumberService : IGenerateEmployeeNumberService

    {
        public string GenerateEmployeeNumber(string lastName, DateOnly dob)
        {
            string cleanName = lastName.Replace(" ", "").ToUpper();
            string prefix = cleanName.Length >= 3
                ? cleanName.Substring(0, 3)
                : cleanName.PadRight(3, '*');

            string randomPart = new Random().Next(0, 100000).ToString("D5");
            string datePart = dob.ToString("ddMMMyyyy").ToUpper();

            return $"{prefix}-{randomPart}-{datePart}";
        }
    }
}
