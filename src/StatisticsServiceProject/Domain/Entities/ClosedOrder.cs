namespace StatisticsServiceProject.Domain.Entities;

public record ClosedOrder(
    long Id,
    long UserId,
    decimal PriceOriginal,
    decimal PriceDiscounted,
    DateTime CreatedAt);