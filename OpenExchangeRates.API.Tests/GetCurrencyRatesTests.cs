using System.Threading.Tasks;
using Agile.API.Client.CallHandling;
using Apis.OpenExchanges;
using Apis.OpenExchanges.Models.Generated;
using NUnit.Framework;

namespace OpenExchangeRates.Tests
{
    [TestFixture]
    public class GetCurrencyRatesTests : CurrencyTestBase
    {

        [Test]
        public async Task IntegrationQuickTest()
        {
            var api = new CurrencyApi();
            api.Initialize("use-a-valid-key");
            var result = await api.GetCurrencyRates();
            Assert.IsNotNull(result);
            Assert.IsTrue(result is JsonCallResult<CurrencyRates>);
            Assert.IsTrue(result.WasSuccessful);
        }

        [Test]
        public async Task InvalidKeyProducesCallErrorResult()
        {
            var api = new CurrencyApi();
            api.Initialize("invalid-api-key");
            var result = await api.GetCurrencyRates();
            Assert.IsNotNull(result);
            Assert.IsTrue(result is CallErrorResult<CurrencyRates>);
            Assert.IsFalse(result.WasSuccessful);

        }
    }
}