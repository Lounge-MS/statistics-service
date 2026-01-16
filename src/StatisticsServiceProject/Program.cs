using DotNetEnv;
using StatisticsServiceProject;

Env.Load(".env");
WebApplicationBuilder builder = WebApplication.CreateBuilder();
builder.Configuration.AddJsonFile("appsettings.json");
builder.Configuration.AddEnvironmentVariables();
IConfigurationSection kafkaSection = builder.Configuration.GetSection("Kafka");

string postgresConnectionString =
    Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING")
    ?? throw new InvalidOperationException("No postgres connection string");

builder.Services
    .SetSnakeCaseEnumSerializing()
    .AddStatisticsServicePostgresMigrations(postgresConnectionString)
    .AddPostgresRepository(postgresConnectionString)
    .AddStatisticsService()
    .AddOrderPaidEventKafkaConsumer(
        kafkaSection,
        kafkaSection.GetSection("OrderPaidEventConsumer"))
    .AddGrpc();

WebApplication app = builder.Build();
app.Services.RunStatisticsServicePostgresMigrations();
app.MapGrpcPresentation();

await app.RunAsync();