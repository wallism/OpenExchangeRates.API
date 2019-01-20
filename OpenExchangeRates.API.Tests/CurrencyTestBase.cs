using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using Apis.OpenExchanges;
using NUnit.Framework;
using OpenExchangeRates.Models.Logging;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;

namespace OpenExchangeRates.API.Tests
{
    public abstract class CurrencyTestBase
    {
        [OneTimeSetUp]
        public void SetupFixture()
        {
            ConfigureLogging();
        }

        [OneTimeTearDown]
        public void TeardownFixture()
        {
            Log.CloseAndFlush();
            Thread.Sleep(1000);
        }

        private static void ConfigureLogging()
        {
            const string product = "testing";

            var assembly = Assembly.GetAssembly(typeof(CurrencyApi));
            var versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);

            var source = new Source
            {
                Product = product,
                Version = versionInfo.FileVersion,
                Hostname = Environment.MachineName,
            };

            var auth = "elastic:elastic-cloud-password";
            var token = Base64Encode(auth);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                // ENRICHERS
                .Enrich.WithThreadId()
                .Enrich.WithProperty("Source", source, destructureObjects: true)
                .Enrich.WithExceptionDetails()
                // SINKS
                .WriteTo.Console()
                .WriteTo.Debug()
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("https://2f21090384c54bff939da4287f54aa68.ap-southeast-1.aws.found.io:9243"))
                {
                    AutoRegisterTemplate = true,
                    AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv6,
                    IndexFormat = "testing-{0:yyyy.MM.dd}",
                    ModifyConnectionSettings = configuration => configuration.GlobalHeaders(new NameValueCollection { { "Authorization", $"Basic {token}" } })
                })
                .CreateLogger();

            Log.Verbose("Logging configured for {@Source}", source);
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }
}