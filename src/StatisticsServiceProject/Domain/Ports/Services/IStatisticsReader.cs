using StatisticsServiceProject.Domain.Entities;

namespace StatisticsServiceProject.Domain.Ports.Services;

public interface IStatisticsReader
{
    Task<DataPoints> GetAverageReceiptAsync(
        TimeRange timeRange,
        CancellationToken cancellationToken = default);

    Task<DataPoints> GetDepositsAsync(
        TimeRange timeRange,
        CancellationToken cancellationToken = default);

    Task<DataPoints> GetProductSalesAsync(
        string productName,
        TimeRange timeRange,
        CancellationToken cancellationToken = default);

    Task<DataPoints> GetExpensesAsync(
        TimeRange timeRange,
        CancellationToken cancellationToken = default);
}