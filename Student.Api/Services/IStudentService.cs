namespace Student.Api.Services;

public interface IStudentService
{
    Task<IEnumerable<Models.Student>> GetAllAsync();
    Task<Models.Student?> GetByIdAsync(int id);
    Task<Models.Student> AddAsync(Models.Student student);
    Task<Models.Student?> UpdateAsync(int id, Models.Student student);
    Task<bool> DeleteAsync(int id);
}
