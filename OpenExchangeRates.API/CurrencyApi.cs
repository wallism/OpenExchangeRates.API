using System;
using System.Threading.Tasks;
using Agile.API.Clients;
using Agile.API.Clients.CallHandling;
using Apis.OpenExchanges.Models.Generated;
using PennedObjects.RateLimiting;
using Serilog;

namespace OpenExchangeRates.API
{
    public class CurrencyApi : ApiBase, ICurrencyApi
    {
        public CurrencyApi(string apiKey, RateLimit rateLimit) : base(apiKey, rateLimit)
        {
            getLatest = PublicGet<CurrencyRates>(MethodPriority.Normal);
        }

        public CurrencyApi(string apiKey) : this(apiKey, RateLimit.Build(1, TimeSpan.FromSeconds(1)))
        {
            getLatest = PublicGet<CurrencyRates>(MethodPriority.Normal);
        }

        protected override string BaseUrl => "https://openexchangerates.org/api";
        public override string ApiId => "CRNC";


        private readonly ApiMethod<CurrencyRates> getLatest;

        public async Task<CallResult<CurrencyRates>> GetCurrencyRates()
        {
            var querystring = $"app_id={ApiKey}";
            var result = await getLatest.Call("latest.json", "", querystring);
            return result;
        }

        protected override void NotifyError<T>(CallResult<T> result)
        {
            const string apiName = "CurrencyApi";
            Log.Error(result.Exception, "{Api} {Uri} {StatusCode} {Elapsed}ms {RawText}", apiName, result.AbsoluteUri, result.StatusCode, result.Elapsed, result.RawText ?? "");
        }


    }
}
