namespace StatisticsServiceProject.Domain.Entities;

public record DataPoints(
    IEnumerable<double> Values,
    DateTime StartTimestamp,
    DateTime EndTimestamp,
    TimeSpan StepTimespan);