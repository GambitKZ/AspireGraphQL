using AspireGraphQL.ServiceDefaults.Data;
using AspireGraphQL.ServiceDefaults.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ServerPart.HotChocolate.Services
{
    public class UniversityService
    {
        private readonly AppDbContext _dbContext;
        public UniversityService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Course>> GetCoursesAsync(string? name)
        {
            if (name.IsNullOrEmpty())
            {
                return await _dbContext.Courses.ToListAsync();

            }
            return await _dbContext.Courses.Where(x => x.Name.Contains(name)).ToListAsync();
        }

        public async Task<Course?> GetCoursesByIdAsync(Guid id)
        {
            return await _dbContext.Courses.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<IEnumerable<Teacher>> GetTeachersAsync(string? name)
        {
            if (name.IsNullOrEmpty())
            {
                return await _dbContext.Teachers.ToListAsync();

            }
            return await _dbContext.Teachers.Where(x => x.Name.Contains(name)).ToListAsync();
        }

        public async Task<Teacher?> GetTeacherByIdAsync(Guid id)
        {
            return await _dbContext.Teachers.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<IEnumerable<Student>> GetStudentsAsync(string? name)
        {
            if (name.IsNullOrEmpty())
            {
                return await _dbContext.Students.ToListAsync();

            }
            return await _dbContext.Students.Where(x => x.Name.Contains(name)).ToListAsync();
        }

        public async Task<IEnumerable<CoursePlan>> GetSchedulerAsync(int id, CancellationToken ct = default)
        {
            return await _dbContext.CoursePlans.Include(x => x.Course).Where(x => x.Id.Equals(id)).ToListAsync(ct);
        }
    }
}
