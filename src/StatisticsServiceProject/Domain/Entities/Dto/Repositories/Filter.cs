namespace StatisticsServiceProject.Domain.Entities.Dto.Repositories;

public record Filter(
    Filter.FilterType Key,
    object Value)
{
    public enum FilterType
    {
        ProductName,
    }
}