using System.Threading.Tasks;
using NUnit.Framework;
using Serilog;

namespace OpenExchangeRates.API.Tests
{
    [TestFixture]
    public class GetCurrencyRatesTests : CurrencyTestBase
    {

        [Test]
        public async Task IntegrationQuickTest()
        {
            var apiKey = Configuration["appSecrets:OpenExchangeRates.ApiKey"];
            Log.Logger.Information(apiKey);

            var api = new CurrencyApi(apiKey);
            var result = await api.GetCurrencyRates();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Value != null);
            Assert.IsTrue(result.WasSuccessful);
            Log.Logger.Debug("{@Value}", result.Value);
        }

        [Test]
        public async Task InvalidKeyProducesCallErrorResult()
        {
            var api = new CurrencyApi("invalid-api-key");
            var result = await api.GetCurrencyRates();
            Assert.IsNotNull(result);
            Assert.IsFalse(result.WasSuccessful);
            Assert.IsTrue(result.Exception != null);

        }
    }
}