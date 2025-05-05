var builder = DistributedApplication.CreateBuilder(args);


var sqlServer = builder.AddSqlServer("sql").WithLifetime(ContainerLifetime.Persistent);
var db = sqlServer.AddDatabase("sqldatabase");

builder.AddProject<Projects.ServerPart>("serverpart")
    .WithReference(db)
    .WaitFor(db);

builder.AddProject<Projects.ServerPart_HotChocolate>("serverpart-hotchocolate")
    .WithReference(db)
    .WaitFor(db);

builder.AddProject<Projects.ServicePart_HotChocolate_ImplementationFirst>("servicepart-hotchocolate-implementationfirst")
    .WithReference(db)
    .WaitFor(db);

builder.Build().Run();
