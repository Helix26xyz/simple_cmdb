using Aspire.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var builder = DistributedApplication.CreateBuilder(args);


var sql = builder.AddSqlServer("DefaultConnection");

var db = sql.AddDatabase("simplecmdb");

var storageMigrationsClient = builder.AddProject<Projects.simplecmdb_StorageMigrations>("storageMigrations")
    .WithReference(db)
    .WaitFor(db);

var apiService = builder.AddProject<Projects.simplecmdb_ApiService>("apiservice")
    .WithReference(db)
    .WithExternalHttpEndpoints();

var cache = builder.AddRedis("cache");

builder.AddProject<Projects.simplecmdb_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(apiService)
    .WaitFor(apiService);

// builder.AddProject<Projects.simplecmdb_DemoClient>("democlient")
//     .WithReference(apiService)
//     .WaitFor(apiService);

builder.Build().Run();
