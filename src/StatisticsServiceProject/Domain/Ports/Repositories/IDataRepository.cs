using StatisticsServiceProject.Domain.Entities;
using StatisticsServiceProject.Domain.Entities.Dto.Repositories;

namespace StatisticsServiceProject.Domain.Ports.Repositories;

public interface IDataRepository
{
    Task AddDataAsync(
        DataType dataType,
        double value,
        string? metainfo,
        DateTime timestamp,
        CancellationToken cancellationToken = default);

    Task<DataPoints> GetDataAsync(
        DataType dataType,
        DataRequestType dataRequestType,
        DateTime startTimestamp,
        DateTime endTimestamp,
        TimeSpan stepTimespan,
        Filter? filter = null,
        CancellationToken cancellationToken = default);
}