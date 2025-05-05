using AspireGraphQL.ServiceDefaults.Models;

namespace ServerPart.HotChocolate.GraphQL.Types;

public class StudentType : ObjectType<Student>
{
    protected override void Configure(IObjectTypeDescriptor<Student> descriptor)
    {
        descriptor.Ignore(f => f.CoursePlan);
        descriptor.Ignore(f => f.CoursePlanId);
    }
}
