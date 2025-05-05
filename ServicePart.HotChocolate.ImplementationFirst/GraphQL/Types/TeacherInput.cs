using AspireGraphQL.ServiceDefaults.Models;

namespace ServicePart.HotChocolate.ImplementationFirst.GraphQL.Types;

public class TeacherInput
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public GenderEnum Gender { get; set; }
}
