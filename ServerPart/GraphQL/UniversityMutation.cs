using GraphQL;
using GraphQL.MicrosoftDI;
using GraphQL.Types;
using ServerPart.Data;
using ServerPart.GraphQL.Types;
using ServerPart.Models;

namespace ServerPart.GraphQL;

public class UniversityMutation : ObjectGraphType
{
    public UniversityMutation()
    {
        Field<CourseType>("updateCourseDescription")
            .Argument<NonNullGraphType<GuidGraphType>>("id")
            .Argument<StringGraphType>("description")
            .Resolve()
            .WithService<AppDbContext>()
            .ResolveAsync(async (context, dbContext) =>
            {
                var id = context.GetArgument<Guid>("id");
                var course = dbContext.Courses.FirstOrDefault(x => x.Id.Equals(id));
                course.Description = context.GetArgument<string>("description");

                await dbContext.SaveChangesAsync();
                return course;
            });

        Field<TeacherType>("addTeacher")
            .Argument<TeacherInputType>("teacher")
            .Resolve()
            .WithService<AppDbContext>()
            .ResolveAsync(async (context, dbContext) =>
            {
                var teacher = context.GetArgument<Teacher>("teacher");

                teacher.Id = Guid.NewGuid();

                dbContext.Add(teacher);

                await dbContext.SaveChangesAsync();

                return teacher;
            });
    }
}
