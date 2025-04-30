using GraphQL.Types;
using ServerPart.Models;

namespace ServerPart.GraphQL.Types
{
    public class StudentType : ObjectGraphType<Student>
    {
        public StudentType()
        {
            Name = "Student";
            Description = "Student in University";
            Field(d => d.Id).Description("Id of Student");
            Field(d => d.Name).Description("Name of Student");
            Field(d => d.Surname).Description("Surname of Student");
            Field<GenderEnumType>("gender")
                .Description("Gender of Student")
                .Resolve(context => context.Source.Gender);
        }
    }
}
