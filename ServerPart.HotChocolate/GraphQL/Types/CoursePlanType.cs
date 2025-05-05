using AspireGraphQL.ServiceDefaults.Models;
using ServerPart.HotChocolate.GraphQL.DataLoaders;
using ServerPart.HotChocolate.Services;

namespace ServerPart.HotChocolate.GraphQL.Types;

public class CoursePlanType : ObjectType<CoursePlan>
{
    protected override void Configure(IObjectTypeDescriptor<CoursePlan> descriptor)
    {
        descriptor.Name("Scheduler");

        descriptor.Ignore(f => f.CourseId);
        descriptor.Ignore(f => f.TeacherId);


        descriptor
            .Field(f => f.Teacher)
            .Resolve<Teacher>(async ctx =>
            {
                var coursePlan = ctx.Parent<CoursePlan>();
                var service = ctx.Service<UniversityService>();

                return await service.GetTeacherByIdAsync(coursePlan.TeacherId);
            });

        descriptor
            .Field(f => f.Students)
            .Resolve<Student[]>(async ctx =>
            {
                var coursePlan = ctx.Parent<CoursePlan>();
                var dataLoader = ctx.Service<StudentsDataLoader>();

                return await dataLoader.LoadAsync(coursePlan.Id, ctx.RequestAborted);
            });
    }
}
