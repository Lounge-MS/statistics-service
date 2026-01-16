using StatisticsServiceProject;

WebApplicationBuilder builder = WebApplication.CreateBuilder();
IConfigurationSection kafkaSection = builder.Configuration.GetSection("Kafka");

builder.Services
    .AddPostgresRepository(
        Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING")
        ?? throw new InvalidOperationException("No postgres connection string"))
    .AddStatisticsService()
    .AddOrderServiceKafkaConsumer(
        kafkaSection,
        kafkaSection.GetSection("OrderServiceConsumer"))
    .AddGrpc();

WebApplication app = builder.Build();
app.MapGrpcPresentation();

await app.RunAsync();