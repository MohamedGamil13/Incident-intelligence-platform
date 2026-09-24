using Domain.Entities.Logs;
using Domain.Enums.Logs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Presistance.Data.Logs
{
    public class LogConfigClass : IEntityTypeConfiguration<Log>
    {
        public void Configure(EntityTypeBuilder<Log> builder)
        {
            builder.HasKey(l => l.Id);
            builder.Property(l => l.Timestamp).IsRequired();
            builder.Property(l => l.LogLevel).IsRequired();
            builder.Property(l => l.Message).IsRequired().HasMaxLength(150);
            builder.Property(l => l.Description).IsRequired().HasMaxLength(1000);
            builder.Property(l => l.TraceId).IsRequired();
            builder.HasOne(l => l.Service).WithMany(s => s.Logs).HasForeignKey(l => l.ServiceId).OnDelete(DeleteBehavior.Cascade);
            builder.HasIndex(l => l.TraceId);
            builder.HasIndex(l => new { l.ServiceId, l.Timestamp });
            builder.HasIndex(l => new { l.ServiceId, l.Timestamp, l.LogLevel })
                   .HasFilter($"\"LogLevel\" IN ({(int)LogLevel.Error}, {(int)LogLevel.Critical})");
        }
    }
}