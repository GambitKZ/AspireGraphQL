using GraphQL.Types;
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

        // Resolved by "Include" in Query
        Field<CourseType>("course")
            .Description("Associated Course of the Planned Course");
        Field<TeacherType>("teacher")
            .Description("Teacher in the Course");


        Field<ListGraphType<StudentType>>("students")
          .Description("Students in the Course");
        //.Resolve()
        //.WithService<AppDbContext>()
        //.ResolveAsync(async (context, dbContext) =>
        //{
        //    return dbContext.Students
        //      .Where(x => x.CoursePlanId.Equals(context.Source.Id)).ToList();
        //});
    }
}
