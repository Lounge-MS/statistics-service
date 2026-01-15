namespace StatisticsServiceProject.Domain.Exceptions;

public class StatisticsServiceException : Exception
{
    public StatisticsServiceException(
        string message = "Exception occured in the statistics service")
        : base(message)
    {
    }
}