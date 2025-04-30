using Bogus;
using GraphQL;
using ServerPart.Data;
using ServerPart.GraphQL;
using ServerPart.Models;

var builder = WebApplication.CreateBuilder(args);

builder.AddSqlServerDbContext<AppDbContext>(
    connectionName: "sqldatabase",
    configureDbContextOptions: options =>
{
    //options.UseSeeding((context, s) =>
    //{
    //    // Seeding doesn't work!!!!
    //    // Gave ""
    //    var faker = new Faker<Student>()
    //    .UseSeed(5312)
    //    .RuleFor(x => x.Id, f => f.Random.Guid())
    //    .RuleFor(x => x.Name, f => f.Person.FirstName)
    //    .RuleFor(x => x.Surname, f => f.Person.LastName)
    //    .RuleFor(x => x.Gender, f => f.Person.Gender.ToString());
    //    var studentsToSeed = faker.Generate(30);

    //    context.Set<Student>().AddRange(studentsToSeed);
    //    context.SaveChanges();
    //});
});

builder.Services.AddSingleton<UniversityQuery>();
builder.Services.AddSingleton<UniversityMutation>();
builder.Services.AddGraphQL(builder =>
    {
        builder
        .AddSchema<AppSchema>()
        .AddSystemTextJson();
    });


builder.AddServiceDefaults();
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Required "GraphQL.Server.Transports.AspNetCore" to work.
// Otherwise you need to write your own controller
app.UseGraphQL("/graphql");

// GraphQL UI with path "/ui/graphiql"
app.UseGraphQLGraphiQL();

app.UseHttpsRedirection();

using (var serviceScope = app.Services.CreateScope())
{
    var dbContext = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();
}

// Fill the Database
app.MapGet("/create", (AppDbContext context) =>
{
    var fakerTeachers = new Faker<Teacher>()
        .UseSeed(151)
        .RuleFor(x => x.Id, f => f.Random.Guid())
        .RuleFor(x => x.Name, f => f.Person.FirstName)
        .RuleFor(x => x.Surname, f => f.Person.LastName)
        .RuleFor(x => x.Gender, f => (f.Person.Gender.ToString().Equals(nameof(GenderEnum.Male))) ? GenderEnum.Male : GenderEnum.Female);
    var teachersToSeed = fakerTeachers.Generate(5);
    context.Set<Teacher>().AddRange(teachersToSeed);

    var fakeCourses = new Faker<Course>()
    .UseSeed(568)
    .RuleFor(x => x.Id, f => f.Random.Guid())
    .RuleFor(x => x.Name, f => f.Commerce.ProductName())
    .RuleFor(x => x.Description, f => "");
    var coursesToSeed = fakeCourses.Generate(5);
    context.Set<Course>().AddRange(coursesToSeed);

    var fakerStudents = new Faker<Student>()
        .UseSeed(5312)
        .RuleFor(x => x.Id, f => f.Random.Guid())
        .RuleFor(x => x.Name, f => f.Person.FirstName)
        .RuleFor(x => x.Surname, f => f.Person.LastName)
        .RuleFor(x => x.Gender, f => (f.Person.Gender.ToString().Equals(nameof(GenderEnum.Male))) ? GenderEnum.Male : GenderEnum.Female);
    var studentsToSeed = fakerStudents.Generate(30);

    var coursePlans = new List<CoursePlan>();

    for (var i = 0; i < 5; i++)
    {
        var coursePlan = new CoursePlan
        {
            //Id = i + 1,
            Name = $"Plan {i + 1}",
            CourseId = coursesToSeed[i].Id,
            TeacherId = teachersToSeed[i].Id,
            Students = new List<Student>()
        };

        var assignedStudents = studentsToSeed.Skip(i * 6).Take(6).ToList();

        foreach (var student in assignedStudents)
        {
            student.CoursePlan = coursePlan;
            coursePlan.Students.Add(student);
        }

        coursePlans.Add(coursePlan);
    }
    context.Set<CoursePlan>().AddRange(coursePlans);
    context.Set<Student>().AddRange(studentsToSeed);

    context.SaveChanges();

    return "done";
});

// Unnecessary with "GraphQL.Server.Transports.AspNetCore"
//app.MapPost("/graphql", (IDocumentExecuter executer, ISchema schema, [FromBody] GraphQLRequest request) =>
//{
//    var result = executer.ExecuteAsync(s =>
//    {
//        s.Schema = schema;
//        s.Query = request.Query;
//        s.Variables = request.Variables;
//        s.OperationName = request.OperationName;
//    });

//    return result;
//});

app.Run();


