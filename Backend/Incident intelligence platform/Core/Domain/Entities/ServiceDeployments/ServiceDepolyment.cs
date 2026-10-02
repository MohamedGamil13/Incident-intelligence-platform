using Domain.Entities.Services;

namespace Domain.Entities.ServiceDeployments
{
    public class ServiceDeployment : BaseEntity<int>
    {

        public string Title { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
        public string DeployedBy { get; set; } = string.Empty;
        public DateTime DeployedAt { get; set; }
        public int ServiceId { get; set; }
        public Service Service { get; set; } = null!;

    }
}