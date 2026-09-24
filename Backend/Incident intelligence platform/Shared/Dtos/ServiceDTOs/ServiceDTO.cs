using System.ComponentModel.DataAnnotations;

namespace Incident_intelligence_platform.DTOs.ServiceDTOs
{
    public class CreateServiceRequestDTO
    {
        [Required(ErrorMessage = "Service name is required.")]
        [MaxLength(100, ErrorMessage = "Service name cannot exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required.")]
        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; } = string.Empty;
    }


    public class UpdateServiceRequestDTO
    {
        [Required(ErrorMessage = "Service name is required.")]
        [MaxLength(100, ErrorMessage = "Service name cannot exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required.")]
        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; } = string.Empty;
    }


    public class GetServiceResponseDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
    }
}