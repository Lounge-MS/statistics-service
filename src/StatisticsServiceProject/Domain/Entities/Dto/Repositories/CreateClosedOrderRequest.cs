namespace StatisticsServiceProject.Domain.Entities.Dto.Repositories;

public record CreateClosedOrderRequest(
    long UserId,
    decimal PriceOriginal,
    decimal PriceDiscounted,
    DateTime ClosedAt);