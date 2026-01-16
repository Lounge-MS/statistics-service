using Grpc.Core;
using GrpcStatisticsService;
using StatisticsServiceProject.Domain.Ports.Services;

namespace StatisticsServiceProject.Infrastructure.Driving.Grpc;

public class GrpcStatisticsPresentation : StatisticsService.StatisticsServiceBase
{
    private readonly IStatisticsReader _reader;
    private readonly IStatisticsWriter _writer;

    public GrpcStatisticsPresentation(
        IStatisticsReader reader,
        IStatisticsWriter writer)
    {
        _reader = reader;
        _writer = writer;
    }

    public override async Task<AddExpenseResponse> AddExpense(
        AddExpenseRequest request,
        ServerCallContext context)
    {
        await _writer.AddExpenseAsync(
            request.AmountInKopecks / 100m,
            request.Timestamp.ToDateTime(),
            context.CancellationToken);

        return new AddExpenseResponse();
    }

    public override async Task<GetAverageReceiptResponse> GetAverageReceipt(
        GetAverageReceiptRequest request,
        ServerCallContext context)
    {
        return new GetAverageReceiptResponse
        {
            Data = (await _reader.GetAverageReceiptAsync(
                request.TimeRange.ToDomain(),
                context.CancellationToken)).ToGrpc(),
        };
    }

    public override async Task<GetDepositsResponse> GetDeposits(
        GetDepositsRequest request,
        ServerCallContext context)
    {
        return new GetDepositsResponse
        {
            Data = (await _reader.GetDepositsAsync(
                request.TimeRange.ToDomain(),
                context.CancellationToken)).ToGrpc(),
        };
    }

    public override async Task<GetProductSalesResponse> GetProductSales(
        GetProductSalesRequest request,
        ServerCallContext context)
    {
        return new GetProductSalesResponse
        {
            Data = (await _reader.GetProductSalesAsync(
                request.ProductName,
                request.TimeRange.ToDomain(),
                context.CancellationToken)).ToGrpc(),
        };
    }

    public override async Task<GetExpensesResponse> GetExpenses(
        GetExpensesRequest request,
        ServerCallContext context)
    {
        return new GetExpensesResponse
        {
            Data = (await _reader.GetExpensesAsync(
                request.TimeRange.ToDomain(),
                context.CancellationToken)).ToGrpc(),
        };
    }
}