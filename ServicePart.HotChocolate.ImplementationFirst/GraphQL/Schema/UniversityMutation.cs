using AspireGraphQL.ServiceDefaults.Data;
using AspireGraphQL.ServiceDefaults.Models;
using GraphQL;
using HotChocolate.Subscriptions;
using ServicePart.HotChocolate.ImplementationFirst.GraphQL.Types;

namespace ServicePart.HotChocolate.ImplementationFirst.GraphQL.Schema;

[Name("Mutation")]
public class UniversityMutation
{
    public async Task<Course> UpdateCourseDescriptionAsync([Service] AppDbContext dbContext, Guid id, string description, ITopicEventSender sender)
    {
        var course = dbContext.Courses.First(x => x.Id.Equals(id));
        course.Description = description;

        await dbContext.SaveChangesAsync();

        await sender.SendAsync("CourseUpdated", id);

        return course;
    }

    public async Task<Teacher> AddTeacherAsync([Service] AppDbContext dbContext, TeacherInput newTeacher, ITopicEventSender sender)
    {
        var teacher = new Teacher()
        {
            Id = Guid.NewGuid(),
            Name = newTeacher.Name,
            Gender = newTeacher.Gender,
            Surname = newTeacher.Surname,
        };

        await dbContext.AddAsync(teacher);

        dbContext.SaveChanges();

        await sender.SendAsync("TeacherAdded", teacher);

        return teacher;
    }
}
