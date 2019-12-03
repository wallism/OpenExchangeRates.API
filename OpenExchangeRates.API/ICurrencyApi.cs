using System.Threading.Tasks;
using Agile.API.Clients.CallHandling;
using OpenExchangeRates.API.Models.Generated;

namespace OpenExchangeRates.API
{
    public interface ICurrencyApi
    {
        Task<CallResult<CurrencyRates>> GetCurrencyRates();
    }
}