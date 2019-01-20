using System.Threading.Tasks;
using Agile.API.Client;
using Agile.API.Client.CallHandling;
using Apis.OpenExchanges.Models.Generated;
using Serilog;

namespace Apis.OpenExchanges
{
    public class CurrencyApi : ApiBase, ICurrencyApi
    {
        public CurrencyApi() 
        {
        }

        protected override string BaseUrl => "https://openexchangerates.org/api";
        public override string Code => "CRNC";


        private readonly ApiMethod getLatest = ApiMethod.Private(false);

        public async Task<ServiceCallResult<CurrencyRates>> GetCurrencyRates()
        {
            var querystring = $"app_id={ApiKey}";
            var result = await Get<CurrencyRates>(getLatest, "latest.json", querystring);
            return result;
        }

        protected override void NotifyErrorHandler<T>(ServiceCallResult<T> errorResult)
        {
            const string apiName = "CurrencyApi";
            Log.Error(errorResult.Exception, "{Api} {StatusCode} {Elapsed}ms", apiName, errorResult.StatusCode, errorResult.Elapsed);
            if(! string.IsNullOrWhiteSpace(errorResult.RawText))
                Log.Warning("{Api} {RawText}", apiName, errorResult.RawText);
        }
    }
}
