using Microsoft.Extensions.Configuration;
using PayPal.Api;
using System.Collections.Generic;
using System.IO;

namespace E_Commerce.Models
{
    public static class PaypalConfiguration
    {
        public static IConfiguration Configuration { get; set; }
        public static readonly string ClientId;
        public static readonly string ClientSecret;

        static PaypalConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();

            var paypalSettings = Configuration.GetSection("PayPal:Settings");
            ClientId = paypalSettings["ClientId"];
            ClientSecret = paypalSettings["ClientSecret"];
        }

        private static string GetAccessToken()
        {
            string accessToken = new OAuthTokenCredential(ClientId, ClientSecret).GetAccessToken();
            return accessToken;
        }

        public static APIContext GetAPIContext()
        {
            APIContext apiContext = new APIContext(GetAccessToken());

            // Convert IConfigurationSection to Dictionary<string, string>
            Dictionary<string, string> configDictionary = new Dictionary<string, string>();
            foreach (var item in Configuration.GetSection("PayPal:Settings").GetChildren())
            {
                configDictionary[item.Key] = item.Value;
            }
            apiContext.Config = configDictionary;

            return apiContext;
        }
    }
}
