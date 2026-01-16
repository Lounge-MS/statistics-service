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
        object? metainfo,
        DataType dataType,
        DateTime timestamp,
        CancellationToken cancellationToken = default)
    {
        return _repository.AddDataAsync(dataType, data, metainfo, timestamp, cancellationToken);
    }

    public Task AddExpenseAsync(decimal amount, DateTime timestamp, CancellationToken cancellationToken = default)
    {
        return AddDataAsync((double)amount, null, DataType.Expense, timestamp, cancellationToken);
    }

    public Task<DataPoints> GetAverageReceiptAsync(
        TimeRange timeRange,
        CancellationToken cancellationToken = default)
    {
        return _repository.GetDataAsync(
            DataType.ClosedOrder,
            DataRequestType.Avg,
            timeRange,
            null,
            cancellationToken);
    }

    public Task<DataPoints> GetDepositsAsync(
        TimeRange timeRange,
        CancellationToken cancellationToken = default)
    {
        return _repository.GetDataAsync(
            DataType.ClosedOrder,
            DataRequestType.Sum,
            timeRange,
            null,
            cancellationToken);
    }

    public Task<DataPoints> GetProductSalesAsync(
        string productName,
        TimeRange timeRange,
        CancellationToken cancellationToken = default)
    {
        return _repository.GetDataAsync(
            DataType.BoughtOrderPosition,
            DataRequestType.Sum,
            timeRange,
            new Filter("productName", productName),
            cancellationToken);
    }

    public Task<DataPoints> GetExpensesAsync(
        TimeRange timeRange,
        CancellationToken cancellationToken = default)
    {
        return _repository.GetDataAsync(
            DataType.Expense,
            DataRequestType.Sum,
            timeRange,
            null,
            cancellationToken);
    }
}