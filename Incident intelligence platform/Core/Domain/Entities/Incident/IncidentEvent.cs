using Domain.Enums.Incident;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Icident
{
    public class IncidentEvent
    {
        public int Id { get; set; }

        public IncidentEventTimeStamp TimeStamp { get; set; }

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        public DateTime Date { get; set; }

        public int IncidentId { get; set; }
    }
}