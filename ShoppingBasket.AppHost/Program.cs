var builder = DistributedApplication.CreateBuilder(args);


var mysql = builder.AddMySql("KantarDB")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithPhpMyAdmin();

var sbdb = mysql.AddDatabase("SBDb");

var myService = builder.AddProject<Projects.ShoppingBasket_API>("api")
    .WithReference(sbdb)
    .WaitFor(sbdb);

builder.Build().Run();