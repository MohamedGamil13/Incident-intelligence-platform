
using System.ComponentModel.DataAnnotations;

namespace Incident_intelligence_platform.DTOs
{
    public class CreateServiceRequestDTO
    {
        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        [MaxLength(500)]
        public required string Description { get; set; }
    }


    public class UpdateServiceRequestDTO
    {
        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        [MaxLength(500)]
        public required string Description { get; set; }
    }


    public class GetServiceResponseDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
    }
}