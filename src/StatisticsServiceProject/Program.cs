using DotNetEnv;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using StatisticsServiceProject;
using StatisticsServiceProject.Infrastructure.Driving.Kafka;

Env.Load("example.env");
WebApplicationBuilder builder = WebApplication.CreateBuilder();
builder.Configuration.AddJsonFile("appsettings.json");
IConfigurationSection kafkaSection = builder.Configuration.GetSection("Kafka");

string postgresConnectionString =
    Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING")
    ?? throw new InvalidOperationException("No postgres connection string");

JsonConvert.DefaultSettings = () => new JsonSerializerSettings
{
    ContractResolver = new DefaultContractResolver
    {
        NamingStrategy = new SnakeCaseNamingStrategy(),
    },
    NullValueHandling = NullValueHandling.Ignore,
};

builder.Services
    .AddStatisticsServicePostgresMigrations(postgresConnectionString)
    .AddPostgresRepository(postgresConnectionString)
    .AddStatisticsService()
    .AddOrderPaidEventTestProducer(
        kafkaSection,
        kafkaSection.GetSection("OrderPaidEventConsumer"))
    .AddOrderPaidEventKafkaConsumer(
        kafkaSection,
        kafkaSection.GetSection("OrderPaidEventConsumer"))
    .AddGrpc();

WebApplication app = builder.Build();
app.Services.RunStatisticsServicePostgresMigrations();
app.MapGrpcPresentation();

await app.RunAsync();