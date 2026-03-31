LJF Payroll System APIThis is a professional-grade .NET 8 Web API designed to manage employee records and compute payroll based on specific scheduling and birthday bonus rules. The project is built with a focus on Clean Architecture, Test-Driven Development (TDD), and SOLID principles.

🏛️ Architecture & Concepts Applied
The application follows a structured approach to ensure maintainability and scalability:

Clean Architecture (Simplified): Logic is decoupled into layers (Controllers, Services, Data, and Mappings). The Controllers handle HTTP, while Services contain the "Business Brain.

"Dependency Injection (DI): Uses built-in .NET DI to inject services (e.g., IPayrollService, IEmployeeService). This makes the code modular and easily testable.

Repository Pattern (via EF Core): The ApplicationDbContext acts as the gateway to the SQL Server database, abstracting the data access logic.

Data Transfer Objects (DTOs): Separate classes are used for Requests and Responses to prevent exposing internal database entities directly to the API consumers.

Global Exception Handling: Implements IExceptionHandler (RFC 7807) to provide standardized, professional error responses across the entire API.

Unit & Integration Testing: Comprehensive test suites using xUnit and Moq to verify business logic without requiring a live database.

🚀 How to Run the Application1. 
Prerequisites.NET 8 SDKSQL Server (LocalDB is used by default)Visual Studio 2022 (or VS Code)

2. SetupClone the repository.
Navigate to the solution folder (where the .sln file is located).
Restore dependencies:Bashdotnet restore
Update the connection string in appsettings.json if you are not using LocalDB.

3. LaunchingVia Visual Studio: Press F5.Via CLI:Bashdotnet run --project LJFpayrollAPI
Access the Swagger UI at https://localhost:[port]/swagger to test the endpoints.

🛠️ Database Migrations
This project uses Entity Framework Core Code-First. 
Follow these steps to sync your database:Open Package Manager Console (PMC) in Visual Studio.

Add a Migration:PowerShellAdd-Migration InitialCreate
Apply to Database:PowerShellUpdate-Database

Note: If you need to start fresh, use Drop-Database before running the update again.

🧪 Running TestsThe solution includes a dedicated test project LJFpayrollAPI.Tests covering various layers.

Using Visual StudioGo to Test > Test Explorer.Click Run All Tests.Using CLIBashdotnet test

What is being tested?

Payroll Calculation: Validates $2\times$ daily rate for scheduled days and the $100\%$ birthday bonus.

ID Generation: Ensures employee numbers follow the NAM-RANDOM-DATE format across cultures.

CRUD Operations: Verifies that employees are correctly saved, updated, and "soft-deleted" in the database.API Controllers: Ensures the correct HTTP status codes (200, 404, 204) are returned.

📝 Key EndpointsGET /api/Employee: List all employees.POST /api/Employee: Register a new employee (auto-generates Employee Number).GET /api/Payroll/compute/{employeeNumber}: 

Calculates total pay for a specific date range.

📂 Folder StructurePlaintext/LJFPayrollSolution
│
├── LJFpayrollAPI.sln
├── .gitignore
│
├── /LJFpayrollAPI          (Main Web API Project)
│   ├── /Controllers        (API Endpoints)
│   ├── /Services           (Business Logic)
│   ├── /Models             (Entities & DTOs)
│   └── /Data               (EF Core Context)
│
└── /LJFpayrollAPI.Tests    (Unit & Integration Tests)
    ├── PayrollServiceTests.cs
    ├── EmployeeServiceTests.cs
    └── ControllerTests.cs
    
👨‍💻 AuthorLeonel - Licensed Electronics Engineer & Full Stack Developer
