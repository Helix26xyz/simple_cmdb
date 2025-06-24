using Microsoft.EntityFrameworkCore;
using simplecmdb.SharedModels.storage;
using simplecmdb.SharedModels.clients;
using simplecmdb.SharedModels.models;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("simplecmdb")));

var baseApiURL = builder.Configuration["BaseApiUrl"];
var name = builder.Configuration["Name"];
var script = builder.Configuration["Script"];
var webhook = builder.Configuration["SimpleCMDB"];

if (string.IsNullOrEmpty(baseApiURL))
{
    throw new ArgumentNullException("BaseApiUrl", "BaseApiUrl is not set in the configuration.");
}

builder.Services.AddHttpClient<SimpleCMDBEventsApiClient>(client =>
{
    client.BaseAddress = new(baseApiURL);
});
builder.Services.AddHttpClient<SimpleCMDBApiClient>(client =>
{
    client.BaseAddress = new(baseApiURL);
});

builder.Services.AddSingleton<SimpleCMDBConsumer>(sp =>
{
    var webhookEventClient = sp.GetRequiredService<SimpleCMDBEventsApiClient>();
    var webhookClient = sp.GetRequiredService<SimpleCMDBApiClient>();
    var webhookConsumer = SimpleCMDBConsumer.CreateAsync(
        webhookEventHttpClient: webhookEventClient,
        webhookHttpClient: webhookClient,
        name: name,
        webhook: webhook,
        script: script
    ).GetAwaiter().GetResult();
    return webhookConsumer;
});

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

var consumer = app.Services.GetRequiredService<SimpleCMDBConsumer>();
await consumer.StartProcessingLoopAsync();

// Terminate the service once the migration is complete
Environment.Exit(0);