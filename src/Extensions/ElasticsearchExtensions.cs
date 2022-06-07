using System;
using System.Linq;
using System.Reflection;
using Fundamentos.Elastic.Kibana.Serilog.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace Fundamentos.Elastic.Kibana.Serilog.Extensions
{
    public static class ElasticsearchExtensions
    {
        public static void AddElasticsearch(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = new ConnectionSettings(new Uri(configuration["ElasticsearchSettings:uri"]));

            var defaultIndex = configuration["ElasticsearchSettings:defaultIndex"];
            if (!string.IsNullOrEmpty(defaultIndex))
                settings = settings.DefaultIndex(defaultIndex);

            var basicAuthUser = configuration["ElasticsearchSettings:username"];
            var basicAuthPassword = configuration["ElasticsearchSettings:password"];

            if (!string.IsNullOrEmpty(basicAuthUser) && !string.IsNullOrEmpty(basicAuthPassword))
                settings = settings.BasicAuthentication(basicAuthUser, basicAuthPassword);

            var client = new ElasticClient(settings);
            AddElasticIndices(client);
            services.AddSingleton<IElasticClient>(client);
        }

        public static void AddElasticIndices(IElasticClient client)
        {
            var types = Assembly
                .GetAssembly(typeof(ElasticBase))
                .GetTypes()
                .Where(t => t.IsSealed && t.IsAssignableTo(typeof(ElasticBase)))
                .ToList();

            foreach(var type in types)  
            {
                var inst = (ElasticBase)Activator.CreateInstance(type);
                if (string.IsNullOrEmpty(inst.Name) || type.GetMethod("Mapping") == null) continue;

                var indexName = inst.Name.ToLower();
                if (!client.Indices.Exists(indexName).Exists)
                    client.Indices.Create(indexName, c => inst.Mapping(c));
            }
        }
    }
}
