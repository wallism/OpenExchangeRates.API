using System.Threading.Tasks;
using Agile.API.Clients.CallHandling;
using Apis.OpenExchanges.Models.Generated;

namespace OpenExchangeRates.API
{
    public interface ICurrencyApi
    {
        Task<CallResult<CurrencyRates>> GetCurrencyRates();
    }
}