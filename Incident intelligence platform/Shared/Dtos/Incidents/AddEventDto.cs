using Domain.Enums.Incident;
using System.ComponentModel.DataAnnotations;

namespace Shared.Dtos.Incidents
{
    public class AddIncidentEventDto
    {
        public IncidentEventTimeStamp TimeStamp { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;
    }
}