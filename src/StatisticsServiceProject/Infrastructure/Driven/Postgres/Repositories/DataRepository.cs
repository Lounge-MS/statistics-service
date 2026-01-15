using Npgsql;
using StatisticsServiceProject.Domain.Entities;
using StatisticsServiceProject.Domain.Entities.Dto.Repositories;
using StatisticsServiceProject.Domain.Ports.Repositories;

namespace StatisticsServiceProject.Infrastructure.Driven.Postgres.Repositories;

public class DataRepository : IDataRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public DataRepository(
        NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public async Task AddDataAsync(
        DataType dataType,
        double value,
        string? metainfo,
        DateTime timestamp,
        CancellationToken cancellationToken = default)
    {
        NpgsqlCommand command = _dataSource.CreateCommand();
        command.CommandText =
            """
            INSERT INTO data_points
                (data_type, value, metainfo, timestamp)
            VALUES(:data_type, :value, :metainfo, :timestamp)
            """;

        command.Parameters.AddWithValue("data_type", dataType);
        command.Parameters.AddWithValue("value", value);
        command.Parameters.AddWithValue("metainfo", metainfo ?? "{}");
        command.Parameters.AddWithValue("timestamp", timestamp);

        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task<DataPoints> GetDataAsync(
        DataType dataType,
        DataRequestType dataRequestType,
        DateTime startTimestamp,
        DateTime endTimestamp,
        TimeSpan stepTimespan,
        Filter? filter = null,
        CancellationToken cancellationToken = default)
    {
        NpgsqlCommand command = _dataSource.CreateCommand();
        command.CommandText =
            """
            WITH intervals AS (
                SELECT generate_series(
                    @start::timestamp,
                    @end::timestamp,
                    @step::interval
                ) AS bucket_start
            )
            SELECT 
                bucket_start,
                bucket_start + @step::interval AS bucket_end,
                AVG(
                    COALESCE(
                        (dp.metainfo->>'quantity')::double precision * dp.value,
                        dp.value
                    )
                ) AS avg_value,
                SUM(
                    COALESCE(
                        (dp.metainfo->>'quantity')::double precision * dp.value,
                        dp.value
                    )
                ) AS sum_value
            FROM intervals i
            LEFT JOIN data_points dp
                ON dp.timestamp >= i.bucket_start
               AND dp.timestamp <  i.bucket_start + @step::interval
               AND dp.data_type = @data_type
               AND (@filter_key IS NULL OR (dp.metainfo->>@filter_key) = @filter_value)
            GROUP BY bucket_start
            ORDER BY bucket_start;
            """;

        command.Parameters.AddWithValue("start", startTimestamp);
        command.Parameters.AddWithValue("end", endTimestamp);
        command.Parameters.AddWithValue("step", stepTimespan);
        command.Parameters.AddWithValue("data_type", dataType);
        command.Parameters.AddWithNullableValue("filter_key", filter?.Key);
        command.Parameters.AddWithNullableValue("filter_value", filter?.Value);

        await using NpgsqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);
        var values = new List<double>();
        while (await reader.ReadAsync(cancellationToken))
        {
            double value = dataRequestType switch
            {
                DataRequestType.Avg => reader.GetDouble(reader.GetOrdinal("avg_value")),
                DataRequestType.Sum => reader.GetDouble(reader.GetOrdinal("sum_value")),
                _ => throw new InvalidOperationException(),
            };

            values.Add(value);
        }

        return new DataPoints(
            values,
            startTimestamp,
            endTimestamp,
            stepTimespan);
    }
}