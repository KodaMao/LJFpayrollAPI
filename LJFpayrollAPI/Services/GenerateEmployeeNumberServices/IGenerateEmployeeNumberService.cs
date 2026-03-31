namespace LJFpayrollAPI.Services.GenerateEmployeeNumberServices
{
    public interface IGenerateEmployeeNumberService
    {
        string GenerateEmployeeNumber(string lastName, DateOnly dateOfBirth);
    }
}
