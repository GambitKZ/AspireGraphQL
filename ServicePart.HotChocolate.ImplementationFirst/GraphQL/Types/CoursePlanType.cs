using AspireGraphQL.ServiceDefaults.Data;
using AspireGraphQL.ServiceDefaults.Models;
using Microsoft.EntityFrameworkCore;
using ServicePart.HotChocolate.ImplementationFirst.GraphQL.DataLoaders;

namespace ServicePart.HotChocolate.ImplementationFirst.GraphQL.Types;

public class CoursePlanType : ObjectType<CoursePlan>
{
    protected override void Configure(IObjectTypeDescriptor<CoursePlan> descriptor)
    {
        descriptor.Ignore(f => f.CourseId);
        descriptor.Ignore(f => f.TeacherId);

        descriptor
        .Field(f => f.Teacher)
        .Resolve<Teacher?>(async ctx =>
        {
            var coursePlan = ctx.Parent<CoursePlan>();
            var dbContext = ctx.Service<AppDbContext>();

            return await dbContext.Teachers.FirstOrDefaultAsync(x => x.Id.Equals(coursePlan.TeacherId));
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
