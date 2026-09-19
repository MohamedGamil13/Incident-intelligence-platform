using Domain.Entities.ServiceDeployments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Presistance.Data.ServiceDeployments
{
    public class ServiceDeploymentsConfigClass : IEntityTypeConfiguration<ServiceDeployment>
    {


        public void Configure(EntityTypeBuilder<ServiceDeployment> builder)
        {
            builder.HasKey(d => d.Id);
            builder.Property(d => d.Title).IsRequired();
            builder.Property(d => d.DeployedAt).IsRequired();
            builder.Property(d => d.DeployedBy).IsRequired();
            builder.Property(d => d.Version).IsRequired();
            builder
                .HasOne(d => d.Service)
                .WithMany(d => d.ServiceDepolyments)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(d => new { d.ServiceId, d.Version });

        }
    }
}
