using AspireGraphQL.ServiceDefaults.Data;
using AspireGraphQL.ServiceDefaults.Models;
using GraphQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ServicePart.HotChocolate.ImplementationFirst.GraphQL.Schema
{
    [Name("Query")]
    public class UniversityQuery
    {
        public IEnumerable<Course> GetCourses([Service] AppDbContext dbContext, string? name)
        {
            if (name.IsNullOrEmpty())
            {
                return [.. dbContext.Courses];
            }

            return dbContext.Courses.Where(c => c.Name.Contains(name));
        }

        public Course? GetCourse([Service] AppDbContext dbContext, Guid id)
        {
            return dbContext.Courses.FirstOrDefault(c => c.Id == id);
        }

        public IEnumerable<Teacher> GetTeachers([Service] AppDbContext dbContext, string? name)
        {
            if (name.IsNullOrEmpty())
            {
                return [.. dbContext.Teachers];
            }

            return dbContext.Teachers.Where(c => c.Name.Contains(name));
        }

        public Teacher? GetTeacher([Service] AppDbContext dbContext, Guid id)
        {
            return dbContext.Teachers.FirstOrDefault(c => c.Id == id);
        }

        public IEnumerable<Student> GetStudents([Service] AppDbContext dbContext, string? name)
        {
            if (name.IsNullOrEmpty())
            {
                return [.. dbContext.Students];
            }

            return dbContext.Students.Where(c => c.Name.Contains(name));
        }

        public Student? GetStudent([Service] AppDbContext dbContext, Guid id)
        {
            return dbContext.Students.FirstOrDefault(c => c.Id == id);
        }

        [GraphQLName("schedulers")]
        public IEnumerable<CoursePlan> GetScheduler([Service] AppDbContext dbContext)
        {
            return [.. dbContext.CoursePlans.Include(x => x.Course)];
        }
    }
}
