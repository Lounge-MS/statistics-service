using StatisticsServiceProject.Domain.Entities;
using StatisticsServiceProject.Domain.Entities.Dto.Repositories;

namespace StatisticsServiceProject.Domain.Ports.Repositories;

public interface IClosedOrdersRepository
{
    Task<ClosedOrder> CreateAsync(CreateClosedOrderRequest request);

    Task<IEnumerable<ClosedOrder>> GetAsync(
        DateTime startTimestamp,
        DateTime endTimestamp,
        DateTime stepTimestamp);
}