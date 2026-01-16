using StatisticsServiceProject.Domain.Entities;
using StatisticsServiceProject.Domain.Entities.Dto.Repositories;

namespace StatisticsServiceProject.Domain.Ports.Repositories;

public interface IDataRepository
{
    Task AddDataAsync(
        DataType dataType,
        double value,
        object? metainfo,
        DateTime timestamp,
        CancellationToken cancellationToken = default);

    Task<DataPoints> GetDataAsync(
        DataType dataType,
        DataRequestType dataRequestType,
        TimeRange timeRange,
        Filter? filter = null,
        CancellationToken cancellationToken = default);
}