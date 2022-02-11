namespace Fundamentos.Elastic.Kibana.Serilog.Models
{
    public abstract class ElasticBase
    {
        public string Message { get; set; }
        public bool Success { get; set; }
    }
}
