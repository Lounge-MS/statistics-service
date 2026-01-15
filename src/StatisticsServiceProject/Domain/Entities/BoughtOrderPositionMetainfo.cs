namespace StatisticsServiceProject.Domain.Entities;

public record BoughtOrderPositionMetainfo(
    long ProductId,
    int Quantity);