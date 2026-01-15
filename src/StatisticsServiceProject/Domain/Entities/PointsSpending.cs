namespace StatisticsServiceProject.Domain.Entities;

public record PointsSpending(
    long Id,
    long OrderId,
    long UserId,
    long PointsSpentAmount,
    DateTime SpentAt);