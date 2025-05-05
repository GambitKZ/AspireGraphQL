using AspireGraphQL.ServiceDefaults.Models;

namespace ServicePart.HotChocolate.ImplementationFirst.GraphQL.Types;

public class StudentType : ObjectType<Student>
{
    protected override void Configure(IObjectTypeDescriptor<Student> descriptor)
    {
        descriptor.Ignore(f => f.CoursePlan);
        descriptor.Ignore(f => f.CoursePlanId);

        // OR set explicitly
        //descriptor.BindFieldsExplicitly();
        //descriptor.Field(f => f.Id);
        //descriptor.Field(f => f.Name);
        //descriptor.Field(f => f.Surname);
        //descriptor.Field(f => f.Gender);
    }
}
