using GraphQL.Types;
using ServerPart.Models;

namespace ServerPart.GraphQL.Types;

public class TeacherType : ObjectGraphType<Teacher>
{
    public TeacherType()
    {
        Name = "Teacher";
        Description = "Teacher in University";
        Field(d => d.Id).Description("Id of Teacher");
        Field(d => d.Name).Description("Name of Teacher");
        Field(d => d.Surname).Description("Surname of Teacher");

        Field<GenderEnumType>("gender")
            .Description("Gender of Teacher")
            .Resolve(context => context.Source.Gender);
    }

}
