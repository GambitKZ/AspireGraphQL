using AspireGraphQL.ServiceDefaults.Data;
using GraphQL;
using GraphQL.MicrosoftDI;
using GraphQL.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ServerPart.GraphQL.Types;

namespace ServerPart.GraphQL
{
    // Explain ALL the Queries
    //https://graphql-dotnet.github.io/docs/getting-started/dependency-injection/
    public class UniversityQuery : ObjectGraphType
    {
        public UniversityQuery()
        {
            Field<ListGraphType<CourseType>>("courses")
                .Description("Return all Courses")
                .Argument<StringGraphType>("name")
                .Resolve()
                .WithService<AppDbContext>()
                .ResolveAsync(async (context, dbContext) =>
                {
                    var name = context.GetArgument<string>("name");
                    if (name.IsNullOrEmpty())
                    {
                        return dbContext.Courses.ToList();

                    }
                    return dbContext.Courses.Where(x => x.Name.Contains(name)).ToList();
                });

            // Service Locator Here. Don't use it.
            Field<CourseType>("course")
                .Description("Return Course by Id")
                .Argument<GuidGraphType>("id")
                .ResolveScoped(context =>
                {
                    using var dbContext = context.RequestServices!.GetRequiredService<AppDbContext>();
                    var id = context.GetArgument<Guid>("id");
                    return dbContext.Courses.FirstOrDefault(x => x.Id.Equals(id));
                });


            Field<ListGraphType<StudentType>>("students")
               .Description("Return all Students")
               .Argument<StringGraphType>("name")
               .Resolve()
               .WithService<AppDbContext>()
               .ResolveAsync(async (context, dbContext) =>
               {
                   var name = context.GetArgument<string>("name");
                   if (name.IsNullOrEmpty())
                   {
                       return dbContext.Students.ToList();
                   }
                   return dbContext.Students.Where(x => x.Name.Contains(name)).ToList();
               });

            Field<StudentType>("student")
                .Description("Return Student by Id")
                .Argument<GuidGraphType>("id")
                .Resolve()
                .WithService<AppDbContext>()
                .ResolveAsync(async (context, dbContext) =>
                {
                    var id = context.GetArgument<Guid>("id");
                    return dbContext.Students.FirstOrDefault(x => x.Id.Equals(id));
                });

            Field<ListGraphType<TeacherType>>("teachers")
               .Description("Return all Teachers")
               .Argument<StringGraphType>("name")
               .Resolve()
                .WithService<AppDbContext>()
                .ResolveAsync(async (context, dbContext) =>
                {
                    var name = context.GetArgument<string>("name");
                    if (name.IsNullOrEmpty())
                    {
                        return dbContext.Teachers.ToList();
                    }
                    return dbContext.Teachers.Where(x => x.Name.Contains(name)).ToList();
                });

            Field<ListGraphType<CoursePlanType>>("schedulers")
               .Description("Return all Planned Courses")
               .Resolve()
                .WithService<AppDbContext>()
                .ResolveAsync(async (context, dbContext) =>
                {
                    return dbContext.CoursePlans
                        .Include(n => n.Course)
                        //.Include(n => n.Teacher)
                        //.Include(n => n.Students)
                        .ToList();
                });

            Field<CoursePlanType>("scheduler")
               .Description("Return all Planned Courses")
               .Argument<IntGraphType>("id")
               .Resolve()
                .WithService<AppDbContext>()
                .ResolveAsync(async (context, dbContext) =>
                {
                    var id = context.GetArgument<int>("id");
                    return dbContext.CoursePlans
                        .Include(n => n.Course)
                        //.Include(n => n.Teacher)
                        //.Include(n => n.Students)
                        .FirstOrDefault(x => x.Id.Equals(id));
                });
        }
    }
}
