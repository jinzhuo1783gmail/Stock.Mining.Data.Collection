using System;
using System.Threading.Tasks;
using RestSharp;

namespace Stock.Symbol.External.Retrievor
{
    public interface IExternalRetrievor
    {
        public Task<bool> CreateRestAsync(string url, int timeout);
        public Task<RestRequest> SetRequestAsync(Method method, RestRequest request = null);
        public Task<TO> RetrieveAsync<TI, TO>(TI Input);
    }
}
