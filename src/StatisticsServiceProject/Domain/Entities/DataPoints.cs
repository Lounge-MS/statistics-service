namespace StatisticsServiceProject.Domain.Entities;

public record DataPoints(
    IEnumerable<double> Values,
    TimeRange TimeRange);