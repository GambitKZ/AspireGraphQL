using GraphQL.Types;

namespace ServerPart.GraphQL.Types;

public class TeacherInputType : InputObjectGraphType
{
    public TeacherInputType()
    {
        Name = "TeacherInput";
        Description = "Teacher model";
        Field<StringGraphType>("name");
        Field<StringGraphType>("surname");
        Field<GenderEnumType>("gender");
    }
}
