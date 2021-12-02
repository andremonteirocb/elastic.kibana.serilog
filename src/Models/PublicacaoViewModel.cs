using System.Collections.Generic;

namespace Fundamentos.Elastic.Kibana.Serilog.Models
{
    public class PublicacaoViewModel
    {
        public PublicacaoViewModel()
        {
            PublicacoesSucesso = new List<Publicacao>();
            PublicacoesFalha = new List<Publicacao>();
        }

        public List<Publicacao> PublicacoesSucesso { get; set; }
        public List<Publicacao> PublicacoesFalha { get; set; }
        public double TotalSucesso { get; set; }
        public double TotalFalha { get; set; }
    }
}
