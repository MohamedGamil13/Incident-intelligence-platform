namespace Domain.Contracts.Logs
{
    public interface ILogsRepo
    {
        Task<double> GetAvgLatencyPerService(int serviceId, int timeWindowInMin = 5);
    }
}