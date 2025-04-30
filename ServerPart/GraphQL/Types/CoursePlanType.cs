using GraphQL.MicrosoftDI;
using GraphQL.Types;
using ServerPart.GraphQL.DataLoaders;
using ServerPart.Models;

namespace ServerPart.GraphQL.Types;

public class CoursePlanType : ObjectGraphType<CoursePlan>
{
    public CoursePlanType()
    {
        Name = "CoursePlan";
        Description = "Planned Course in University";

        Field(d => d.Id).Description("Id of the Planned Course");
        Field(d => d.Name).Description("Name of the Planned Course");

        // Resolved by "Include" in Query by EntityFramework
        Field<CourseType>("course")
            .Description("Associated Course of the Planned Course");

        // Resolve by DataLoader
        Field<TeacherType, Teacher>("teacher")
            .Description("Teacher in the Course")
            .Resolve()
            .WithService<TeacherDataLoader>()
            .ResolveAsync((context, loader) =>
            {
                return loader.LoadAsync(context.Source.TeacherId);
            });


        // Resolve by DataLoader with Cache
        Field<ListGraphType<StudentType>, IEnumerable<Student>>("students")
            .Description("Students in the Course")
            .Resolve()
            .WithService<StudentsDataLoader>()
            .ResolveAsync((context, loader) =>
            {
                return loader.LoadAsync(context.Source.Id);
            });
        // Directly from dbContext
        //.WithService<AppDbContext>()
        //.ResolveAsync(async (context, dbContext) =>
        //{
        //    return dbContext.Students
        //        .Where(x => x.CoursePlanId.Equals(context.Source.Id)).ToList();
        //});
    }
}
