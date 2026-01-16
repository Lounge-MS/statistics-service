using Google.Protobuf.WellKnownTypes;
using StatisticsServiceProject.Domain.Entities;

namespace StatisticsServiceProject.Infrastructure.Driving.Grpc;

public static class MappingExtensions
{
    public static TimeRange ToDomain(
        this GrpcStatisticsService.TimeRange timeRange)
    {
        return new TimeRange(
            StartTimestamp: timeRange.StartTimestamp.ToDateTime(),
            EndTimestamp: timeRange.EndTimestamp.ToDateTime(),
            StepTimespan: timeRange.Step.ToTimeSpan());
    }

    public static GrpcStatisticsService.DataPoints ToGrpc(
        this DataPoints dataPoints)
    {
        var ans = new GrpcStatisticsService.DataPoints
        {
            TimeRange = new GrpcStatisticsService.TimeRange
            {
                StartTimestamp = Timestamp.FromDateTime(dataPoints.TimeRange.StartTimestamp),
                EndTimestamp = Timestamp.FromDateTime(dataPoints.TimeRange.EndTimestamp),
                Step = Duration.FromTimeSpan(dataPoints.TimeRange.StepTimespan),
            },
        };
        ans.Values.Add(dataPoints.Values);
        return ans;
    }
}