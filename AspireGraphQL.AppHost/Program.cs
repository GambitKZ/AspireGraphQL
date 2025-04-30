var builder = DistributedApplication.CreateBuilder(args);


var sqlServer = builder.AddSqlServer("sql").WithLifetime(ContainerLifetime.Persistent);
var db = sqlServer.AddDatabase("sqldatabase");

builder.AddProject<Projects.ServerPart>("serverpart")
    .WithReference(db)
    .WaitFor(db);

builder.Build().Run();
