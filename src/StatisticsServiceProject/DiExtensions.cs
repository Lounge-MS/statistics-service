using FluentMigrator.Runner;
using Itmo.Dev.Platform.Kafka.Extensions;
using Npgsql;
using OrderService;
using StatisticsServiceProject.Domain.Entities.Dto.Repositories;
using StatisticsServiceProject.Domain.Ports.Repositories;
using StatisticsServiceProject.Domain.Ports.Services;
using StatisticsServiceProject.Domain.Services;
using StatisticsServiceProject.Infrastructure.Driven.Postgres.Migrations;
using StatisticsServiceProject.Infrastructure.Driven.Postgres.Repositories;
using StatisticsServiceProject.Infrastructure.Driving.Grpc;
using StatisticsServiceProject.Infrastructure.Driving.Kafka;
using StatisticsServiceProject.Tools;
using System.ComponentModel.DataAnnotations;

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
        dataSourceBuilder.MapEnum<DataType>();
        dataSourceBuilder.MapEnum<Filter.FilterType>();
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

    public static IServiceCollection SetSnakeCaseEnumSerializing(
        this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddSingleton<IEnumConverter, SnakeCaseEnumConverter>();
    }
}