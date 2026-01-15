using Npgsql;
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
        object? value)
    {
        return collection.AddWithValue(key, value ?? DBNull.Value);
    }
}