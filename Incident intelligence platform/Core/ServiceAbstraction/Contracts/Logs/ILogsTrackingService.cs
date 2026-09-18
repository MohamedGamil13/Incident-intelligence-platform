namespace ServiceAbstraction.Contracts.Logs
{
    public interface ILogsTrackingService // Event Threshold Handler
    {






    }
}
// Watch Logs and When Log.error Exceed 5 in Short Time it will Auto Generate new Incident



/*
When New log Created we will Send notification 
and make ILogsTrackingService Reicive This Notification and Strat Timer with Each Error Log if Time Exceed 5 min For example and when we have a 5 Error within our time constraint (5 min in this example) we will generate new incident 















*/