using Minimal_API.Models;

var builder = WebApplication.CreateBuilder(args);

 // Cấu hình logging
builder.Logging.ClearProviders();   
builder.Logging.AddConsole();
builder.Logging.AddDebug();    

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = null;
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Đăng ký EmployeeRepository vào DI container (singleton)
builder.Services.AddSingleton<IEmployeeRepository, EmployeeRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Đăng ký global error handling middleware
app.UseMiddleware<ErrorHandlerMiddleware>();
// In-memory list simulating a database of employees
var employeeList = new List<Employee>
{
    new Employee { Id = 1, Name = "John Doe", Position = "Software Engineer", Salary = 60000 },
    new Employee { Id = 2, Name = "Jane Smith", Position = "Project Manager", Salary = 80000 }
};
// -------------------- Minimal API Endpoints --------------------

// GET /employees/{id}
// -------------------- Endpoint với try-catch --------------------
            
// GET /employees
app.MapGet("/employees", (IEmployeeRepository repo, ILogger<Program> logger, HttpContext httpContext) =>
{
    try
    {
        logger.LogInformation("Fetching all employees");

        var query = httpContext.Request.Query;
        if (query.ContainsKey("causeError") &&
            bool.TryParse(query["causeError"], out bool causeError) && causeError)
        {
            throw new NullReferenceException("Simulated null reference exception for testing.");
        }

        var employees = repo.GetAllEmployees();
        return Results.Ok(employees);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error occurred while fetching all employees");
        return Results.Problem(
            detail: "An error occurred while processing your request.",
            statusCode: 500,
            instance: httpContext.Request.Path,
            title: "Internal Server Error");
    }
});

// GET /employees/{id}
app.MapGet("/employees/{id}", (int id, IEmployeeRepository repo, ILogger<Program> logger) =>
{
    try
    {
        logger.LogInformation($"Fetching employee with ID: {id}");
        var employee = repo.GetEmployeeById(id);

        if (employee == null)
        {
            logger.LogWarning($"Employee with ID {id} not found");
            return Results.NotFound(new { Message = $"Employee with ID {id} not found." });
        }

        return Results.Ok(employee);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, $"Error occurred while fetching employee with ID {id}");
        return Results.Problem(
            detail: "An error occurred while processing your request.",
            statusCode: 500,
            title: "Internal Server Error");
    }
});

// POST /employees
app.MapPost("/employees", (Employee newEmployee, IEmployeeRepository repo, ILogger<Program> logger) =>
{
    try
    {
        if (!ValidationHelper.TryValidate(newEmployee, out var errors))
        {
            logger.LogWarning($"Validation failed: {string.Join(", ", errors.Select(e => e.ErrorMessage))}");
            return Results.BadRequest(new
            {
                Message = "Validation Failed",
                Errors = errors.Select(e => e.ErrorMessage)
            });
        }

        var createdEmployee = repo.AddEmployee(newEmployee);
        logger.LogInformation($"Employee created with ID {createdEmployee.Id}");
        return Results.Created($"/employees/{createdEmployee.Id}", createdEmployee);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error occurred while creating employee");
        return Results.Problem(
            detail: "An error occurred while processing your request.",
            statusCode: 500,
            title: "Internal Server Error");
    }
});

// PUT /employees/{id}
app.MapPut("/employees/{id}", (int id, Employee updatedEmployee, IEmployeeRepository repo, ILogger<Program> logger) =>
{
    try
    {
        if (!ValidationHelper.TryValidate(updatedEmployee, out var errors))
        {
            logger.LogWarning($"Validation failed while updating employee {id}: {string.Join(", ", errors.Select(e => e.ErrorMessage))}");
            return Results.BadRequest(new
            {
                Message = "Validation Failed",
                Errors = errors.Select(e => e.ErrorMessage)
            });
        }

        var employee = repo.UpdateEmployee(id, updatedEmployee);
        if (employee == null)
        {
            logger.LogWarning($"Employee with ID {id} not found");
            return Results.NotFound(new { Message = $"Employee with ID {id} not found." });
        }

        logger.LogInformation($"Employee with ID {id} updated");
        return Results.Ok(employee);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, $"Error occurred while updating employee {id}");
        return Results.Problem(
            detail: "An error occurred while processing your request.",
            statusCode: 500,
            title: "Internal Server Error");
    }
});

// DELETE /employees/{id}
app.MapDelete("/employees/{id}", (int id, IEmployeeRepository repo, ILogger<Program> logger) =>
{
    try
    {
        var deleted = repo.DeleteEmployee(id);
        if (!deleted)
        {
            logger.LogWarning($"Employee with ID {id} not found");
            return Results.NotFound(new { Message = $"Employee with ID {id} not found." });
        }

        logger.LogInformation("Employee with ID {EmployeeId} deleted", id);
        return Results.NoContent();
    }
    catch (Exception ex)
    {
        logger.LogError(ex, $"Error occurred while deleting employee {id}");
        return Results.Problem(
            detail: "An error occurred while processing your request.",
            statusCode: 500,
            title: "Internal Server Error");
    }
});
app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
