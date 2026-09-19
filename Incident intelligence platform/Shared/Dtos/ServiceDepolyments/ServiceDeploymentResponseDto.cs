namespace Shared.Dtos.ServiceDepolyments
{
    public class ServiceDeploymentResponseDto
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
        public string DeployedBy { get; set; } = string.Empty;
        public DateTime DeployedAt { get; set; }
    }
}
