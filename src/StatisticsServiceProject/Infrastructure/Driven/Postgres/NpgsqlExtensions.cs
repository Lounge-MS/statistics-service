using Npgsql;
using NpgsqlTypes;
using StatisticsServiceProject.Domain.Exceptions;

namespace StatisticsServiceProject.Infrastructure.Driven.Postgres;

public static class NpgsqlExtensions
{
    public static async Task ReadOrThrowAsync(
        this NpgsqlDataReader dataReader)
    {
        if (!await dataReader.ReadAsync())
        {
            throw new EntityNotFoundException();
        }
    }

    public static NpgsqlParameter AddWithNullableValue(
        this NpgsqlParameterCollection collection,
        string key,
        NpgsqlDbType dbType,
        object? value)
    {
        if (value == null)
        {
            return collection.AddWithValue(key, dbType, DBNull.Value);
        }

        return collection.AddWithValue(key, dbType, value);
    }
}