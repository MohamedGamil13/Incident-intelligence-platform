namespace Incident_intelligence_platform.DTOs.IcidentEventDTOs
{
    public class GetTimeLineRequest
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}