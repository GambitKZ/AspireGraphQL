using AspireGraphQL.ServiceDefaults.Models;
using GraphQL.Types;

namespace ServerPart.GraphQL.Types;

public class CourseType : ObjectGraphType<Course>
{
    public CourseType()
    {
        Name = "Course";
        Description = "Course in University";
        Field(d => d.Id).Description("Id of the Course");
        Field(d => d.Name).Description("Name of the Course");
        Field(d => d.Description).Description("Description of the Course");
    }
}
