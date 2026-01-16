using FluentMigrator.Runner;
using Itmo.Dev.Platform.Kafka.Extensions;
using Npgsql;
using OrderService;
using StatisticsServiceProject.Domain.Ports.Repositories;
using StatisticsServiceProject.Domain.Ports.Services;
using StatisticsServiceProject.Domain.Services;
using StatisticsServiceProject.Infrastructure.Driven.Postgres.Migrations;
using StatisticsServiceProject.Infrastructure.Driven.Postgres.Repositories;
using StatisticsServiceProject.Infrastructure.Driving.Grpc;
using StatisticsServiceProject.Infrastructure.Driving.Kafka;

namespace StatisticsServiceProject;

public static class DiExtensions
{
    public static IServiceCollection AddStatisticsService(
        this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddScoped<IStatisticsReader, StatisticsService>()
            .AddScoped<IStatisticsWriter, StatisticsService>();
    }

    public static IServiceCollection AddPostgresRepository(
        this IServiceCollection serviceCollection,
        string connectionString)
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
        return serviceCollection
            .AddSingleton(dataSourceBuilder.Build())
            .AddScoped<IDataRepository, DataRepository>();
    }

    public static IServiceCollection AddOrderPaidEventKafkaConsumer(
        this IServiceCollection serviceCollection,
        IConfigurationSection kafkaSection,
        IConfigurationSection kafkaConsumerSection)
    {
        return serviceCollection.AddPlatformKafka(builder => builder
            .ConfigureOptions(kafkaSection)
            .AddConsumer(b => b
                .WithKey<OrderSuccessfulKey>()
                .WithValue<OrderSuccessfulValue>()
                .WithConfiguration(kafkaConsumerSection)
                .DeserializeKeyWithNewtonsoft()
                .DeserializeValueWithNewtonsoft()
                .HandleWith<OrderPaidEventKafkaConsumerHandler>()));
    }

    public static IServiceCollection AddOrderPaidEventTestProducer(
        this IServiceCollection serviceCollection,
        IConfigurationSection kafkaSection,
        IConfigurationSection kafkaConsumerSection)
    {
        return serviceCollection.AddPlatformKafka(builder => builder
            .ConfigureOptions(kafkaSection)
            .AddProducer(b => b
                .WithKey<OrderSuccessfulKey>()
                .WithValue<OrderSuccessfulValue>()
                .WithConfiguration(kafkaConsumerSection)
                .SerializeKeyWithNewtonsoft()
                .SerializeValueWithNewtonsoft()));
    }

    public static void MapGrpcPresentation(
        this IEndpointRouteBuilder serviceProvider)
    {
        serviceProvider.MapGrpcService<GrpcStatisticsPresentation>();
    }

    public static IServiceCollection AddStatisticsServicePostgresMigrations(
        this IServiceCollection serviceCollection,
        string connectionString)
    {
        return serviceCollection
            .AddFluentMigratorCore()
            .ConfigureRunner(r => r
                .AddPostgres()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(typeof(CreateDataPointsTable).Assembly)
                .For.Migrations());
    }

    public static void RunStatisticsServicePostgresMigrations(
        this IServiceProvider serviceProvider)
    {
        IMigrationRunner runner = serviceProvider.GetRequiredService<IMigrationRunner>();
        runner.MigrateUp();
    }
}