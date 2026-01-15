namespace StatisticsServiceProject.Domain.Entities;

public record BoughtOrderPosition(
    long Id,
    long UserId,
    long OrderId,
    long ProductId,
    string ProductName,
    long Quantity,
    DateTime BoughtAt);