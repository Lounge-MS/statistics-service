namespace StatisticsServiceProject.Domain.Entities;

public record TimeRange(
    DateTime StartTimestamp,
    DateTime EndTimestamp,
    TimeSpan StepTimespan);