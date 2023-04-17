using BLL.Interfaces;
using Braintree;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace BLL.Services
{
    public class BraintreeService : IBraintreeService
    {
        private readonly IConfiguration _config;

        public BraintreeService(IConfiguration config)
        {
            _config = config;
        }

        public IBraintreeGateway CreateGateway()
        {
            var braintreeGateway = _config.GetSection("BraintreeGateway");

            var newGateway = new BraintreeGateway()
            {
                Environment = Braintree.Environment.SANDBOX,
                MerchantId = braintreeGateway["MerchantId"],
                PublicKey = braintreeGateway["PublicKey"],
                PrivateKey = braintreeGateway["PrivateKey"]
            };

            return newGateway;
        }

        public IBraintreeGateway GetGateway()
        {
            return CreateGateway();
        }
    }
}
