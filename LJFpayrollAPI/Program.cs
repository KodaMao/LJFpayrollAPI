using LJFpayrollAPI.Data;
using LJFpayrollAPI.Handlers;
using LJFpayrollAPI.Services.EmployeeServices;
using LJFpayrollAPI.Services.GenerateEmployeeNumberServices;
using LJFpayrollAPI.Services.PayrollServices;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IGenerateEmployeeNumberService, GenerateEmployeeNumberService>();
builder.Services.AddScoped<IPayrollService, PayrollService>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
