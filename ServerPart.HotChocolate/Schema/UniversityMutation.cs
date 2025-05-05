using AspireGraphQL.ServiceDefaults.Data;
using AspireGraphQL.ServiceDefaults.Models;
using HotChocolate.Subscriptions;
using ServerPart.HotChocolate.GraphQL.Types;

namespace ServerPart.HotChocolate.Schema
{
    public class UniversityMutation : ObjectType
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Name(OperationTypeNames.Mutation);

            descriptor.Field("updateCourseDescription")
                .Argument("id", a => a.Type<UuidType>())
                .Argument("description", a => a.Type<StringType>())
                .Resolve<Course>(async ctx =>
                {

                    var dbContext = ctx.Service<AppDbContext>();
                    var id = ctx.ArgumentValue<Guid>("id");
                    var description = ctx.ArgumentValue<string>("description");

                    var course = dbContext.Courses.First(x => x.Id.Equals(id));
                    course.Description = description;

                    dbContext.SaveChanges();

                    return course;
                });

            descriptor.Field("addTeacher")
                .Argument("input", a => a.Type<TeacherInputType>())
                .Resolve<Teacher>(async ctx =>
                {
                    var teacher = ctx.ArgumentValue<Teacher>("input");

                    teacher.Id = Guid.NewGuid();

                    var dbContext = ctx.Service<AppDbContext>();

                    await dbContext.AddAsync(teacher);

                    dbContext.SaveChanges();

                    var sender = ctx.Service<ITopicEventSender>();

                    // Name is important
                    await sender.SendAsync("TeacherAdded", teacher);

                    return teacher;
                });
        }
    }
}
