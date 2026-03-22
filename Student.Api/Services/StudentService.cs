using Microsoft.EntityFrameworkCore;
namespace Student.Api.Services;

public class StudentService(AppDbContext context) : IStudentService
{
    public async Task<IEnumerable<Models.Student>> GetAllAsync()
        => await context.Students.ToListAsync();

    public async Task<Models.Student?> GetByIdAsync(int id)
        => await context.Students.FindAsync(id);

    public async Task<Models.Student> AddAsync(Models.Student student)
    {
        context.Students.Add(student);
        await context.SaveChangesAsync();
        return student;
    }

    public async Task<Models.Student?> UpdateAsync(int id, Models.Student updated)
    {
        var student = await context.Students.FindAsync(id);
        if (student is null) return null;

        student.FirstName = updated.FirstName;
        student.LastName = updated.LastName;
        student.Email = updated.Email;
        student.Age = updated.Age;

        await context.SaveChangesAsync();
        return student;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var student = await context.Students.FindAsync(id);
        if (student is null) return false;

        context.Students.Remove(student);
        await context.SaveChangesAsync();
        return true;
    }
}