using AspireGraphQL.ServiceDefaults.Data;
using ServicePart.HotChocolate.ImplementationFirst.GraphQL.DataLoaders;
using ServicePart.HotChocolate.ImplementationFirst.GraphQL.Schema;
using ServicePart.HotChocolate.ImplementationFirst.GraphQL.Types;

var builder = WebApplication.CreateBuilder(args);

builder.AddSqlServerDbContext<AppDbContext>(
    connectionName: "sqldatabase",
    configureDbContextOptions: options =>
    {
    });

builder.Services.AddMemoryCache();

builder.Services.AddGraphQLServer()
    .InitializeOnStartup()
    .AddInMemorySubscriptions()
    .AddQueryType<UniversityQuery>()
    .AddMutationType<UniversityMutation>()
    .AddSubscriptionType<UniversitySubscription>()
    .AddType<StudentType>()
    .AddType<CoursePlanType>()
    .AddDataLoader<StudentsDataLoader>()
    ;

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseWebSockets();
app.MapGraphQL();

app.Run();
