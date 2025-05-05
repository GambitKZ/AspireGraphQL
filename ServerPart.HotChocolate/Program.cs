using AspireGraphQL.ServiceDefaults.Data;
using ServerPart.HotChocolate.GraphQL.DataLoaders;
using ServerPart.HotChocolate.GraphQL.Types;
using ServerPart.HotChocolate.Schema;
using ServerPart.HotChocolate.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddSqlServerDbContext<AppDbContext>(
    connectionName: "sqldatabase",
    configureDbContextOptions: options =>
    {
    });

builder.Services.AddMemoryCache();

builder.Services.AddScoped<UniversityService>();

builder.Services.AddGraphQLServer()
    .AddQueryType<UniversityQuery>()
    .AddMutationType<UniversityMutation>()
    // Pick up that Instead of "Student" -> return "StudentType"
    .AddType<StudentType>()
    .AddType<CoursePlanType>()
    .AddDataLoader<StudentsDataLoader>()
    .AddSubscriptionType<UniversitySubscription>()
    .InitializeOnStartup()
    .AddInMemorySubscriptions()
    ;

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

app.UseHttpsRedirection();

app.UseWebSockets();
app.MapGraphQL();

app.Run();