using AspireGraphQL.ServiceDefaults.Models;

namespace ServerPart.HotChocolate.GraphQL.Types
{
    public class TeacherInputType : InputObjectType<Teacher>
    {
        protected override void Configure(IInputObjectTypeDescriptor<Teacher> descriptor)
        {
            descriptor.Ignore(f => f.Id);
        }
    }
}
