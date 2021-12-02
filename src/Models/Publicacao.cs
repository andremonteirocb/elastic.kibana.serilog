using System;

namespace Fundamentos.Elastic.Kibana.Serilog.Models
{
    public abstract class ElasticBase
    {
        public string Message { get; set; }
        public bool Success { get; set; }
        public double TotalSucesso => Success ? 1 : 0;
        public double TotalFalha => Success ? 0 : 1;
    }

    public sealed class Publicacao : ElasticBase
    {
        public Publicacao() { }
        public Publicacao(bool success, string message, Guid? id = null)
        {
            PublicacaoId = id == null ? Guid.NewGuid() : id.Value;
            Success = success;
            Message = message;
        }

        public Guid PublicacaoId { get; set; }
    }
}
