using System;
using System.Net.Http;
using System.Threading.Tasks;
using Agile.API.Clients;
using Agile.API.Clients.CallHandling;
using OpenExchangeRates.API.Models.Generated;
using PennedObjects.RateLimiting;
using Serilog;

namespace OpenExchangeRates.API
{
    public class CurrencyApi : ApiBase, ICurrencyApi
    {
        public CurrencyApi(string apiKey, RateLimit rateLimit) : base(apiKey, rateLimit)
        {
            getLatest = PrivateGet<CurrencyRates>(MethodPriority.Normal);
        }

        public CurrencyApi(string apiKey) : this(apiKey, RateLimit.Build(1, TimeSpan.FromSeconds(1)))
        {
        }

        protected override string BaseUrl => "https://openexchangerates.org/api";
        public override string ApiId => "CRNC";

        protected override void SetPrivateRequestProperties(HttpRequestMessage request, string method, object rawPayload = null, string propsWithNonce = "")
        {
            // nothing to do for OpenExchange api, creds provided as querystring
        }

        private readonly ApiMethod<CurrencyRates> getLatest;

        public async Task<CallResult<CurrencyRates>> GetCurrencyRates()
        {
            var result = await getLatest.Call("latest.json", "", $"app_id={ApiKey}");
            return result;
        }

        protected override void NotifyError<T>(CallResult<T> result)
        {
            const string apiName = "CurrencyApi";
            Log.Error(result.Exception, "{Api} {Uri} {StatusCode} {Elapsed}ms {RawText}", apiName, result.AbsoluteUri, result.StatusCode, result.Elapsed, result.RawText ?? "");
        }


    }
}
