using System.Threading.Tasks;
using Agile.API.Client.CallHandling;
using Apis.OpenExchanges.Models.Generated;

namespace Apis.OpenExchanges
{
    public interface ICurrencyApi
    {
        Task<ServiceCallResult<CurrencyRates>> GetCurrencyRates();
    }
}