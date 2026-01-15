using StatisticsServiceProject.Domain.Entities;
using StatisticsServiceProject.Domain.Entities.Dto.Repositories;

namespace StatisticsServiceProject.Domain.Ports.Repositories;

public interface IBoughtOrderPositionsRepository
{
    Task<BoughtOrderPosition> CreateAsync(CreateBoughtOrderPositionRequest request);

    Task<IEnumerable<BoughtOrderPosition>> GetAsync(
        DateTime startTimestamp,
        DateTime endTimestamp,
        DateTime stepTimestamp,
        long? productId = null,
        string? productName = null);
}