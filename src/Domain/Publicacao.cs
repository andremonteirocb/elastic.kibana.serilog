using System;

namespace Fundamentos.Elastic.Kibana.Serilog.Models
{
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
