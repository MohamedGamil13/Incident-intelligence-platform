namespace Shared.Dtos.ServiceDTOs
{
    public class AnalyzeServiceRequest
    {
        public int ServiceId { get; set; }
        public int IncidentTimeWindow { get; set; } = 5;
        public int ServiceErrorTimeWindow { get; set; } = 5;
        public int MaxIncidentsPerService { get; set; } = 5;
        public int MaxErrorsPerService { get; set; } = 5;
        public long MaxLatancy { get; set; } = 5;
    }
}
