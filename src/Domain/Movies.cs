using System;

namespace Fundamentos.Elastic.Kibana.Serilog.Models
{
    public sealed class Movies : ElasticBase
    {
        public Movies() { }
        public Movies(string id, int year, string title)
        {
            base.Id = id;
            this.year = year;
            this.title = title;
        }

        public int year { get; set; }
        public string title { get; set; }
    }
}
