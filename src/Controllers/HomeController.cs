using Fundamentos.Elastic.Kibana.Serilog.Models;
using Elasticsearch.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Fundamentos.Elastic.Kibana.Serilog.Controllers
{
    public class HomeController : Controller
    {
        private readonly IElasticClient _elasticClient;
        private readonly ILogger<HomeController> _logger;
        public HomeController(
            ILogger<HomeController> logger,
            IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("LogInformation");

            return View();
        }

        public IActionResult Privacy()
        {
            _logger.LogWarning("LogWarning");

            return View();
        }

        public async Task<IActionResult> Inserir(int quantidade = 500, Guid? publicacaoId = null)
        {
            //var query = new QueryContainerDescriptor<Publicacao>().MatchAll();
            //var response = await _elasticClient.DeleteByQueryAsync<Publicacao>(q => q
            //    .Query(_ => query)
            //    .Index("publicacao")
            //);

            //if (!response.IsValid)
            //    throw new Exception(response.ServerError?.ToString(), response.OriginalException);

            publicacaoId = publicacaoId == null ? Guid.NewGuid() : publicacaoId;
            for (var i = 0; i <= quantidade; i++)
            {
                var success = i % 2 == 0 ? true : false;
                var message = success ? "Parabéns e-mail enviado com sucesso" : "Falha ao enviar o e-mail";
                var publicacao = new Publicacao(success, message, publicacaoId);
                await _elasticClient.IndexDocumentAsync(publicacao, new System.Threading.CancellationToken());
            }

            //var search = new SearchDescriptor<Publicacao>("publicacao").MatchAll();
            //var result = await _elasticClient.SearchAsync<Publicacao>(search);

            //if (!result.IsValid)
            //    throw new Exception(result.ServerError?.ToString(), result.OriginalException);

            return RedirectToAction("Logs", new { publicacaoId = publicacaoId });
        }

        [HttpGet]
        public IActionResult Logs(string publicacaoId = "d0d91d15-23a1-4d4a-855c-c699b2dddd00")
        {
            ViewBag.PublicacaoId = publicacaoId;
            return View(new PublicacaoViewModel());
        }

        [HttpPost]
        public IActionResult ObterLogs(string publicacaoId = "d0d91d15-23a1-4d4a-855c-c699b2dddd00")
        {
            var model = new PublicacaoViewModel();
            //var filters = new List<Func<QueryContainerDescriptor<Publicacao>, QueryContainer>>();
            //filters.Add(fq => fq.Terms(t => t.Field(f => f.Success).Terms(true)));
            //filters.Add(fq => fq.Terms(t => t.Field(f => f.PublicacaoId).Terms(new[] { "2b480ccf-39c3-4aef-9278-7e05f8d7a789" })));
            //filters.Add(fq => fq.MatchPhrase(p => p.Field(field).Query("true")));
            //filters.Add(fq => fq.MatchPhrase(p => p.Field("message").Query("Parabéns e-mail enviado com sucesso")));

            //var result = _elasticClient.Search<Publicacao>(s =>
            //    s.Index("publicacao")
            //        .Query(q => q.Bool(b => b.Filter(filters)))
            //        .Size(100));

            var query = new QueryContainerDescriptor<Publicacao>().MatchPhrase(p => p.Field(x => x.Success).Query("true"));
            var query2 = new QueryContainerDescriptor<Publicacao>().MatchPhrase(p => p.Field(x => x.PublicacaoId).Query(publicacaoId));
            var result = _elasticClient.Search<Publicacao>(s =>
                s.Index("publicacao")
                  .Query(_ => query && query2)
                  .Size(5)
                  .TrackTotalHits());

            if (!result.IsValid)
                throw new Exception(result.ServerError?.ToString(), result.OriginalException);

            model.TotalSucesso = result.Total;
            model.PublicacoesSucesso = result.Documents.ToList();

            //filters = new List<Func<QueryContainerDescriptor<Publicacao>, QueryContainer>>();
            //filters.Add(fq => fq.Terms(t => t.Field(f => f.Success).Terms(false)));
            //filters.Add(fq => fq.Terms(t => t.Field(f => f.PublicacaoId).Terms(new[] { "c971ba91-28dc-4a2d-90b0-91d98e57dd46" })));
            //filters.Add(fq => fq.MatchPhrase(p => p.Field(field).Query("false")));
            //filters.Add(fq => fq.MatchPhrase(p => p.Field("message").Query("Falha ao enviar o e-mail")));

            result = _elasticClient.Search<Publicacao>(s =>
                s.Index("publicacao")
                  .Query(q =>
                    q.MatchPhrase(p => p.Field(x => x.Success).Query("false")) &&
                    q.MatchPhrase(p => p.Field(x => x.PublicacaoId).Query(publicacaoId))
                  )
                  .Size(5)
                  .TrackTotalHits());

            if (!result.IsValid)
                throw new Exception(result.ServerError?.ToString(), result.OriginalException);

            model.TotalFalha = result.Total;
            model.PublicacoesFalha = result.Documents.ToList();

            //query = new QueryContainerDescriptor<Publicacao>().MatchPhrase(p => p.Field(x => x.PublicacaoId).Query(publicacaoId));
            //result = _elasticClient.Search<Publicacao>(s =>
            //    s.Index("publicacao")
            //        .Query(_ => query)
            //        .Aggregations(s => s.Sum("TotalSucesso", sa => sa.Field(o => o.TotalSucesso))
            //            .Sum("TotalFalha", sa => sa.Field(p => p.TotalFalha))));

            //if (!result.IsValid)
            //    throw new Exception(result.ServerError?.ToString(), result.OriginalException);

            //var totalSucesso = NestExtensions.ObterBucketAggregationDouble(result.Aggregations, "TotalSucesso");
            //var totalFalha = NestExtensions.ObterBucketAggregationDouble(result.Aggregations, "TotalFalha");

            //model.TotalFalha = totalFalha;
            //model.TotalSucesso = totalSucesso;

            //query = new QueryContainerDescriptor<Publicacao>().Match(p => p.Field(field).Query(value));
            //result = _elasticClient.Search<Publicacao>(s =>
            //     s.Index("publicacao")
            //      .Query(_ => query));

            //query = new QueryContainerDescriptor<Publicacao>().Term(p => field, value);
            //result = _elasticClient.Search<Publicacao>(s =>
            //    s.Index("publicacao")
            //        .Query(_ => query));

            //query = new QueryContainerDescriptor<Publicacao>().MatchPhrasePrefix(p => p.Field(field).Query(value));
            //result = _elasticClient.Search<Publicacao>(s =>
            //    s.Index("publicacao")
            //        .Query(_ => query));

            //query = new QueryContainerDescriptor<Publicacao>().Wildcard(p => p.Field(f => f.PublicacaoId).Value(field + "*"));
            //result = _elasticClient.Search<Publicacao>(s =>
            //    s.Index("publicacao")
            //        .Query(_ => query));

            //query = new QueryContainerDescriptor<Publicacao>().Wildcard(p => p.Field(f => f.PublicacaoId).Value(field + "*"));
            //result = _elasticClient.Search<Publicacao>(s =>
            //    s.Index("publicacao")
            //        .Query(_ => query));

            //var search = new SearchDescriptor<Publicacao>("publicacao").Query(q => q.Term(x => x.PublicacaoId.ToString().ToLowerInvariant(), field.ToString().ToLowerInvariant()));
            //result = _elasticClient.Search<Publicacao>(search);

            ViewBag.PublicacaoId = publicacaoId;
            return View("Logs", model);
        }

        public IActionResult Error()
        {
            _logger.LogError("Testando View de Error");
            return RedirectToAction("Index");
        }
    }

    public static class NestExtensions
    {
        public static double ObterBucketAggregationDouble(AggregateDictionary agg, string bucket)
        {
            return agg.BucketScript(bucket).Value.HasValue ? agg.BucketScript(bucket).Value.Value : 0;
        }
    }
}
