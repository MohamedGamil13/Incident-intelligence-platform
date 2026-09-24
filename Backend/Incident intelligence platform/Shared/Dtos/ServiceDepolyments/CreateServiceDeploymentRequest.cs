using System.ComponentModel.DataAnnotations;

namespace Shared.Dtos.ServiceDepolyments
{
    public class CreateServiceDeploymentRequest
    {
        [Required(ErrorMessage = "Title is required.")]
        [MaxLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Version is required.")]
        [MaxLength(20, ErrorMessage = "Version cannot exceed 20 characters.")]
        public string Version { get; set; } = string.Empty;

        [Required(ErrorMessage = "Deployer name is required.")]
        [MaxLength(50, ErrorMessage = "Deployer name cannot exceed 50 characters.")]
        public string DeployedBy { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "ServiceId must be greater than 0.")]
        public int ServiceId { get; set; }
    }
}