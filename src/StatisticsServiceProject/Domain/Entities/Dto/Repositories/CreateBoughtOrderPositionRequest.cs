namespace StatisticsServiceProject.Domain.Entities.Dto.Repositories;

public record CreateBoughtOrderPositionRequest(
    long UserId,
    long OrderId,
    long ProductId,
    string ProductName,
    long Quantity);