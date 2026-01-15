namespace StatisticsServiceProject.Domain.Entities;

public record Expense(
    long Id,
    long Amount,
    DateTime CreatedAt);