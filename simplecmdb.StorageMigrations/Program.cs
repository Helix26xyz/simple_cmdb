using Microsoft.EntityFrameworkCore;
using simplecmdb.StorageMigrations.src;
using simplecmdb.SharedModels.storage;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("simplecmdb")));

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapDefaultEndpoints();

app.MapControllers();

var migrationService = new DatabaseMigrationService(
    builder.Configuration.GetConnectionString("simplecmdb"),
    Path.Combine(AppContext.BaseDirectory, "migrations")
);

await migrationService.EnsureDatabaseMigratedAsync();

// Terminate the service once the migration is complete
Environment.Exit(0);
