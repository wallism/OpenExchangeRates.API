using System.Threading.Tasks;
using Agile.API.Client.CallHandling;
using Apis.OpenExchanges.Models.Generated;

namespace OpenExchangeRates.API
{
    public interface ICurrencyApi
    {
        Task<ServiceCallResult<CurrencyRates>> GetCurrencyRates();
    }
}