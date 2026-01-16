using Grpc.Core;
using Grpc.Core.Interceptors;
using StatisticsServiceProject.Domain.Exceptions;

namespace StatisticsServiceProject.Infrastructure.Driving.Grpc;

public class ExceptionInterceptor : Interceptor
{
    private readonly ILogger<ExceptionInterceptor> _logger;

    public ExceptionInterceptor(
        ILogger<ExceptionInterceptor> logger)
    {
        _logger = logger;
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await continuation(request, context);
        }
        catch (EntityNotFoundException ex)
        {
            throw new RpcException(new Status(StatusCode.NotFound, ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");

            throw new RpcException(new Status(
                StatusCode.Internal,
                "Internal server error: " + ex.Message));
        }
    }
}