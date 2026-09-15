using System.ComponentModel.DataAnnotations;

namespace Shared.Dtos.IcidentEventDTOs
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