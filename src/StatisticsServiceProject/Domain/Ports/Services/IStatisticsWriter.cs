using StatisticsServiceProject.Domain.Entities;

namespace StatisticsServiceProject.Domain.Ports.Services;

public interface IStatisticsWriter
{
    Task AddDataAsync(
        double data,
        object? metainfo,
        DataType dataType,
        DateTime timestamp,
        CancellationToken cancellationToken = default);

    Task AddExpenseAsync(
        decimal amount,
        DateTime timestamp,
        CancellationToken cancellationToken = default);
}