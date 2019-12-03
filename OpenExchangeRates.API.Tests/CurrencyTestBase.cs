using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using OpenExchangeRates.API.Models.Logging;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;

namespace OpenExchangeRates.API.Tests
{
    public abstract class CurrencyTestBase
    {
        static CurrencyTestBase()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsecrets.development.json", optional: false);
            Configuration = builder.Build();

            ConfigureLogging();
        }

        protected static IConfigurationRoot Configuration { get; }

        [OneTimeSetUp]
        public void SetupFixture()
        {
        }


        [OneTimeTearDown]
        public void TeardownFixture()
        {
            Log.CloseAndFlush();
            Thread.Sleep(200);
        }

        private static void ConfigureLogging()
        {
            var assembly = Assembly.GetAssembly(typeof(CurrencyApi));
            if(assembly == null)
                throw new Exception("failed to load assembly in ConfigureLogging");

            var versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            var product = assembly.GetName().Name ?? "api.test";

            var source = new Source
            {
                Product = product,
                Version = versionInfo.FileVersion,
                Hostname = Environment.MachineName,
            };
            
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                // ENRICHERS
                .Enrich.WithThreadId()
                .Enrich.WithProperty("Source", source, destructureObjects: true)
                .Enrich.WithExceptionDetails()
                // SINKS
                .WriteTo.Console()
                .WriteTo.Debug()
                .CreateLogger();

            Log.Information("Logging configured for {@Source}", source);
        }

    }
}