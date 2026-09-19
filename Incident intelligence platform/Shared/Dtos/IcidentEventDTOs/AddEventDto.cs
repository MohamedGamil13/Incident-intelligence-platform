using Domain.Enums.Incident;
using System.ComponentModel.DataAnnotations;

namespace Shared.Dtos.IcidentEventDTOs
{
    public class AddIncidentEventDto
    {
        [Required(ErrorMessage = "Timestamp is required.")]
        public IncidentEventTimeStamp? TimeStamp { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [MaxLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required.")]
        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; } = string.Empty;
    }
}