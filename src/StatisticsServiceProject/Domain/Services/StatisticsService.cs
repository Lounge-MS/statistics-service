using StatisticsServiceProject.Domain.Entities;
using StatisticsServiceProject.Domain.Entities.Dto.Repositories;
using StatisticsServiceProject.Domain.Ports.Repositories;
using StatisticsServiceProject.Domain.Ports.Services;

namespace StatisticsServiceProject.Domain.Services;

public class StatisticsService : IStatisticsReader, IStatisticsWriter
{
    private readonly IDataRepository _repository;

    public StatisticsService(
        IDataRepository repository)
    {
        _repository = repository;
    }

    public Task AddDataAsync(
        double data,
        string? metainfo,
        DataType dataType,
        DateTime timestamp,
        CancellationToken cancellationToken = default)
    {
        return _repository.AddDataAsync(dataType, data, metainfo, timestamp, cancellationToken);
    }

    public Task<DataPoints> GetAverageReceiptAsync(
        DateTime startTimestamp,
        DateTime endTimestamp,
        TimeSpan stepTimespan,
        CancellationToken cancellationToken = default)
    {
        return _repository.GetDataAsync(
            DataType.ClosedOrder,
            DataRequestType.Avg,
            startTimestamp,
            endTimestamp,
            stepTimespan,
            null,
            cancellationToken);
    }

    public Task<DataPoints> GetDepositsAsync(
        DateTime startTimestamp,
        DateTime endTimestamp,
        TimeSpan stepTimespan,
        CancellationToken cancellationToken = default)
    {
        return _repository.GetDataAsync(
            DataType.ClosedOrder,
            DataRequestType.Sum,
            startTimestamp,
            endTimestamp,
            stepTimespan,
            null,
            cancellationToken);
    }

    public Task<DataPoints> GetProductSalesAsync(
        long productId,
        DateTime startTimestamp,
        DateTime endTimestamp,
        TimeSpan stepTimespan,
        CancellationToken cancellationToken = default)
    {
        return _repository.GetDataAsync(
            DataType.BoughtOrderPosition,
            DataRequestType.Sum,
            startTimestamp,
            endTimestamp,
            stepTimespan,
            new Filter("product_id", productId),
            cancellationToken);
    }

    public Task<DataPoints> GetExpensesAsync(
        DateTime startTimestamp,
        DateTime endTimestamp,
        TimeSpan stepTimespan,
        CancellationToken cancellationToken = default)
    {
        return _repository.GetDataAsync(
            DataType.Expense,
            DataRequestType.Sum,
            startTimestamp,
            endTimestamp,
            stepTimespan,
            null,
            cancellationToken);
    }
}