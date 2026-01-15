using StatisticsServiceProject.Domain.Entities;

namespace StatisticsServiceProject.Domain.Ports.Services;

public interface IStatisticsWriter
{
    Task AddDataAsync(
        double data,
        string? metainfo,
        DataType dataType,
        DateTime timestamp,
        CancellationToken cancellationToken = default);
}