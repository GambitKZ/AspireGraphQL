using AspireGraphQL.ServiceDefaults.Data;
using AspireGraphQL.ServiceDefaults.Models;
using Microsoft.EntityFrameworkCore;
using ServerPart.HotChocolate.Services;

namespace ServerPart.HotChocolate.Schema
{
    public class UniversityQuery : ObjectType
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Name(OperationTypeNames.Query);

            descriptor
                .Field("courses")
                .Argument("name", a => a.Type<StringType>())
                .Resolve<IEnumerable<Course>>(async ctx => await ctx
                .Service<UniversityService>()
                .GetCoursesAsync(ctx.ArgumentValue<string>("name")));

            descriptor
                .Field("course")
                .Argument("id", a => a.Type<NonNullType<UuidType>>())
                .Resolve<Course?>(async ctx => await ctx
                .Service<UniversityService>()
                .GetCoursesByIdAsync(ctx.ArgumentValue<Guid>("id")));

            descriptor
                .Field("teachers")
                .Argument("name", a => a.Type<StringType>())
                .Resolve<IEnumerable<Teacher>>(async ctx => await ctx
                .Service<UniversityService>()
                .GetTeachersAsync(ctx.ArgumentValue<string>("name")));

            descriptor
                .Field("teacher")
                .Argument("id", a => a.Type<NonNullType<UuidType>>())
                .ResolveWith<UniversityService>(r => r.GetTeacherByIdAsync(default));

            descriptor
                .Field("students")
                .Argument("name", a => a.Type<StringType>())
                .Resolve<IEnumerable<Student>>(async (ctx, ct) =>
                {

                    var dbContext = ctx.Service<AppDbContext>();
                    var name = ctx.ArgumentValue<string>("name");

                    if (name is null)
                    {
                        return await dbContext.Students.ToListAsync();

                    }
                    return await dbContext.Students.Where(x => x.Name.Contains(name)).ToListAsync(ct);
                });

            descriptor
                .Field("schedulers")
                .Resolve<IEnumerable<CoursePlan>>(async ctx =>
                {
                    var dbContext = ctx.Service<AppDbContext>();

                    return await dbContext.CoursePlans.ToListAsync();
                });

            descriptor
                .Field("scheduler")
                .Argument("id", a => a.Type<NonNullType<IntType>>())
                .Resolve<IEnumerable<CoursePlan>>(async ctx =>
                {
                    var service = ctx.Service<UniversityService>();
                    var id = ctx.ArgumentValue<int>("id");

                    return await service.GetSchedulerAsync(id, ctx.RequestAborted);
                });
        }
    }
}
