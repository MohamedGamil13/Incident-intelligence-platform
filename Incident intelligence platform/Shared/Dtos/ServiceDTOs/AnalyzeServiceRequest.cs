namespace Shared.Dtos.ServiceDTOs
{
    public class AnalyzeServiceRequest : AnalyzeAllServicesRequest
    {

        public int ServiceId { get; set; }
        public AnalyzeServiceRequest(AnalyzeAllServicesRequest request, int serviceId)
        {
            IncidentTimeWindow = request.IncidentTimeWindow;
            ServiceErrorTimeWindow = request.ServiceErrorTimeWindow;
            MaxIncidentsPerService = request.MaxIncidentsPerService;
            MaxErrorsPerService = request.MaxErrorsPerService;
            MaxLatancy = request.MaxLatancy;

            ServiceId = serviceId;
        }
    }

}
