using Newtonsoft.Json;
using Npgsql;
using NpgsqlTypes;
using StatisticsServiceProject.Domain.Entities;
using StatisticsServiceProject.Domain.Entities.Dto.Repositories;
using StatisticsServiceProject.Domain.Ports.Repositories;
using StatisticsServiceProject.Tools;

namespace StatisticsServiceProject.Infrastructure.Driven.Postgres.Repositories;

public class DataRepository : IDataRepository
{
    private readonly NpgsqlDataSource _dataSource;
    private readonly IEnumConverter _enumConverter;

    public DataRepository(
        NpgsqlDataSource dataSource,
        IEnumConverter enumConverter)
    {
        _dataSource = dataSource;
        _enumConverter = enumConverter;
    }

    public async Task AddDataAsync(
        DataType dataType,
        double value,
        object? metainfo,
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

        command.Parameters.AddWithValue("data_type", _enumConverter.ConvertToString(dataType));
        command.Parameters.AddWithValue("value", value);
        command.Parameters.AddWithValue("metainfo", NpgsqlDbType.Jsonb, JsonConvert.SerializeObject(metainfo));
        command.Parameters.AddWithValue("timestamp", timestamp);

        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task<DataPoints> GetDataAsync(
        DataType dataType,
        DataRequestType dataRequestType,
        TimeRange timeRange,
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
                COALESCE(
                    AVG(
                        COALESCE(
                            (dp.metainfo->>'quantity')::double precision * dp.value,
                            dp.value
                        )
                    ), 0
                ) as avg_value,
                COALESCE(
                    SUM(
                        COALESCE(
                            (dp.metainfo->>'quantity')::double precision * dp.value,
                            dp.value
                        )
                    ), 0
                ) as sum_value
            FROM intervals i
            LEFT JOIN data_points dp
                ON dp.timestamp >= i.bucket_start
               AND dp.timestamp <  i.bucket_start + @step::interval
               AND dp.data_type = @data_type
               AND (@filter_key IS NULL OR (dp.metainfo->>@filter_key) = @filter_value)
            GROUP BY bucket_start
            ORDER BY bucket_start;
            """;

        command.Parameters.AddWithValue("start", timeRange.StartTimestamp);
        command.Parameters.AddWithValue("end", timeRange.EndTimestamp);
        command.Parameters.AddWithValue("step", timeRange.StepTimespan);
        command.Parameters.AddWithValue("data_type", _enumConverter.ConvertToString(dataType));
        if (filter != null)
        {
            command.Parameters.AddWithValue("filter_key", _enumConverter.ConvertToString(filter.Key));
            command.Parameters.AddWithValue("filter_value", filter.Value);
        }
        else
        {
            command.Parameters.AddWithValue("filter_key", NpgsqlDbType.Varchar, DBNull.Value);
            command.Parameters.AddWithValue("filter_value", NpgsqlDbType.Varchar, DBNull.Value);
        }

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

        return new DataPoints(values, timeRange);
    }
}