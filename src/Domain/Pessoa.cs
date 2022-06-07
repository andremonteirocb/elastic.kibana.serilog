using Nest;

namespace Fundamentos.Elastic.Kibana.Serilog.Models
{
    public sealed class Pessoa : ElasticBase
    {
        public string cidade { get; set; }
        public string estado { get; set; }
        public string formacao { get; set; }
        public string interesses { get; set; }
        public string nome { get; set; }
        public string pais { get; set; }

        public override string Name => "pessoa";
        public override CreateIndexDescriptor Mapping(CreateIndexDescriptor c)
        {
            return c
              .Map<Pessoa>(mm => mm
                  .Properties(p => p
                      .Text(t => t
                          .Name(n => n.cidade)
                          .Analyzer("portuguese")
                          .Fields(ff => ff
                              .Keyword(k => k
                                .Name("original")
                              )
                          )
                      )
                  )
                  .Properties(p => p
                      .Text(t => t
                          .Name(n => n.formacao)
                          .Analyzer("portuguese")
                          .Fields(ff => ff
                              .Keyword(k => k
                                .Name("original")
                              )
                          )
                      )
                  )
                  .Properties(p => p
                      .Text(t => t
                          .Name(n => n.interesses)
                          .Analyzer("portuguese")
                      )
                  )
                  .Properties(p => p
                      .Text(t => t
                          .Name(n => n.nome)
                          .Analyzer("portuguese")
                          .Fields(ff => ff
                              .Keyword(k => k
                                .Name("original")
                              )
                          )
                      )
                  )
                  .Properties(p => p
                      .Text(t => t
                          .Name(n => n.pais)
                          .Analyzer("portuguese")
                          .Fields(ff => ff
                              .Keyword(k => k
                                .Name("original")
                              )
                          )
                      )
                  )
                  .Properties(p => p
                    .Keyword(t => t
                        .Name(n => n.estado)
                    )
                  )
              );
        }
    }
}
