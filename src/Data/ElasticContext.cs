using Fundamentos.Elastic.Kibana.Serilog.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Fundamentos.Elastic.Kibana.Serilog.Data
{
    public class ElasticContext : DbContext
    {
        public ElasticContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Publicacao> Publicacao { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PublicacaoConfiguration());
            //modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
