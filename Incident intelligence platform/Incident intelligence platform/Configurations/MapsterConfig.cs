using Incident_intelligence_platform.DTOs;
using Incident_intelligence_platform.Models;
using Mapster;

public static class MapsterConfig
{
    public static void RegisterMapsterConfiguration(this IServiceCollection services)
    {
        TypeAdapterConfig<Incident, GetIncidentResponseDTO>
            .NewConfig()
            .Map(dest => dest.ServiceName, src => src.Service != null ? src.Service.Name : null);
    }
}