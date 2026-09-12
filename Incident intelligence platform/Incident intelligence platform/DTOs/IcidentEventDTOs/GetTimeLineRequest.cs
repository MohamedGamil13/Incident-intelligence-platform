using System.ComponentModel.DataAnnotations;

namespace Incident_intelligence_platform.DTOs.IcidentEventDTOs
{
    public class GetTimeLineRequest
    {
        [Required]
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int IncidentId { get; set; }

    }
}
