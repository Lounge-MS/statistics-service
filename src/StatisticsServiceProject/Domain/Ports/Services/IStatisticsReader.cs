using StatisticsServiceProject.Domain.Entities;

namespace StatisticsServiceProject.Domain.Ports.Services;

public interface IStatisticsReader
{
    Task<DataPoints> GetAverageReceiptAsync(
        DateTime startTimestamp,
        DateTime endTimestamp,
        TimeSpan stepTimespan,
        CancellationToken cancellationToken = default);

    Task<DataPoints> GetDepositsAsync(
        DateTime startTimestamp,
        DateTime endTimestamp,
        TimeSpan stepTimespan,
        CancellationToken cancellationToken = default);

    Task<DataPoints> GetProductSalesAsync(
        long productId,
        DateTime startTimestamp,
        DateTime endTimestamp,
        TimeSpan stepTimespan,
        CancellationToken cancellationToken = default);

    Task<DataPoints> GetExpensesAsync(
        DateTime startTimestamp,
        DateTime endTimestamp,
        TimeSpan stepTimespan,
        CancellationToken cancellationToken = default);
}