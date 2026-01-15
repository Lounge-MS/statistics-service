using StatisticsServiceProject.Domain.Entities;
using StatisticsServiceProject.Domain.Entities.Dto.Repositories;

namespace StatisticsServiceProject.Domain.Ports.Repositories;

public interface IPointsSpendingsRepository
{
    Task<PointsSpending> CreateAsync(CreatePointsSpendingRequest request);

    Task<IEnumerable<PointsSpending>> GetAsync(
        DateTime startTimestamp,
        DateTime endTimestamp,
        DateTime stepTimestamp);
}