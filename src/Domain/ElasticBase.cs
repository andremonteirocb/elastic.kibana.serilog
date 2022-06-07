using Nest;

namespace Fundamentos.Elastic.Kibana.Serilog.Models
{
    public abstract class ElasticBase
    {
        public string Id { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }

        public virtual string Name { get; }
        public virtual CreateIndexDescriptor Mapping(CreateIndexDescriptor c)
        {
            return null;
        }
    }
}
