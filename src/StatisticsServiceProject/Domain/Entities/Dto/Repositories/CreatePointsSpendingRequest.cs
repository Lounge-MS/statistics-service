namespace StatisticsServiceProject.Domain.Entities.Dto.Repositories;

public record CreatePointsSpendingRequest(
    long OrderId,
    long UserId,
    long PointsSpentAmount,
    DateTime SpentAt);