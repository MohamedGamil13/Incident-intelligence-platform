using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Incident_intelligence_platform.Models
{
    public class IncidentEventConfigClass : IEntityTypeConfiguration<IncidentEvent>
    {
        public void Configure(EntityTypeBuilder<IncidentEvent> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.TimeStamp)
                .IsRequired();

            builder.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.Date)
                .IsRequired();

            builder.Property(e => e.IncidentId)
                .IsRequired();


            builder.HasOne<Incident>()
                .WithMany()
                .HasForeignKey(e => e.IncidentId)
                .OnDelete(DeleteBehavior.Cascade);


            builder.HasIndex(e => new { e.IncidentId, e.Date });
        }
    }
}