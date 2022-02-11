using Fundamentos.Elastic.Kibana.Serilog.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fundamentos.Elastic.Kibana.Serilog.Data
{
    public class PublicacaoConfiguration : IEntityTypeConfiguration<Publicacao>
    {
        public void Configure(EntityTypeBuilder<Publicacao> builder)
        {
            builder.HasKey(p => p.PublicacaoId);
            builder.Property(p => p.Message)
                .IsRequired()
                .HasMaxLength(1000);
        }
    }
}