using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Student.Api.Models;
using Student.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// Add Swagger/OpenAPI
// Configure Swagger/OpenAPI
// Use the standard Swagger setup for .NET 8
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Student API", Version = "v1" });
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IStudentService, StudentService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

// Seed the database with 20 students on startup if the table is empty
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        // Apply any pending migrations
        context.Database.Migrate();

        if (!context.Students.Any())
        {
            var rnd = new Random();
            var firstNames = new[] { "Alex", "Jamie", "Taylor", "Jordan", "Morgan", "Casey", "Riley", "Avery", "Parker", "Quinn" };
            var lastNames = new[] { "Smith", "Johnson", "Brown", "Williams", "Jones", "Miller", "Davis", "Garcia", "Rodriguez", "Wilson" };
            var students = new List<Student.Api.Models.Student>();
            for (int i = 1; i <= 20; i++)
            {
                var fn = firstNames[rnd.Next(firstNames.Length)];
                var ln = lastNames[rnd.Next(lastNames.Length)];
                var email = $"{fn.ToLower()}.{ln.ToLower()}{i}@example.com";
                var age = rnd.Next(18, 30);
                students.Add(new Student.Api.Models.Student { FirstName = fn, LastName = ln, Email = email, Age = age });
            }

            context.Students.AddRange(students);
            context.SaveChanges();
        }
    }
    catch (Exception ex)
    {
        // If seeding fails, log the error to the console. Avoid throwing to keep app startup resilient.
        Console.WriteLine($"An error occurred seeding the DB: {ex.Message}");
    }
}

app.Run();
