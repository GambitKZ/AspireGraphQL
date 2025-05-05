using AspireGraphQL.ServiceDefaults.Models;

namespace ServicePart.HotChocolate.ImplementationFirst.GraphQL.Schema;

public class UniversitySubscription
{
    [Subscribe]
    public Teacher TeacherAdded([EventMessage] Teacher teacher) => teacher;

    [Subscribe]
    [Topic("CourseUpdated")]
    public Guid CourseUpdated([EventMessage] Guid id) => id;
}
