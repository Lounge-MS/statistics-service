using Itmo.Dev.Platform.Kafka.Consumer;
using OrderService;
using StatisticsServiceProject.Domain.Entities;
using StatisticsServiceProject.Domain.Ports.Services;

namespace StatisticsServiceProject.Infrastructure.Driving.Kafka;

public class OrderPaidEventKafkaConsumerHandler
    : IKafkaConsumerHandler<OrderSuccessfulKey, OrderSuccessfulValue>
{
    private readonly IStatisticsWriter _statisticsWriter;

    public OrderPaidEventKafkaConsumerHandler(
        IStatisticsWriter statisticsWriter)
    {
        _statisticsWriter = statisticsWriter;
    }

    public async ValueTask HandleAsync(
        IEnumerable<IKafkaConsumerMessage<OrderSuccessfulKey, OrderSuccessfulValue>> messages,
        CancellationToken cancellationToken = default)
    {
        foreach (IKafkaConsumerMessage<OrderSuccessfulKey, OrderSuccessfulValue> message in messages)
        {
            await _statisticsWriter.AddDataAsync(
                message.Value.OrderPrice,
                null,
                DataType.ClosedOrder,
                message.Timestamp.DateTime,
                cancellationToken);

            foreach (Item productItem in message.Value.OrderItems)
            {
                await _statisticsWriter.AddDataAsync(
                    productItem.Quantity,
                    new
                    {
                        ProductName = message.Timestamp.DateTime,
                    },
                    DataType.BoughtOrderPosition,
                    message.Timestamp.DateTime,
                    cancellationToken);
            }
        }
    }
}