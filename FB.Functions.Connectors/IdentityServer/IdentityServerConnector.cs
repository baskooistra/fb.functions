using CommunityToolkit.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FB.Functions.Connectors.IdentityServer
{
    public class IdentityServerConnector(
        IConfiguration configuration, 
        IHttpClientFactory httpClientFactory,
        ILogger<IdentityServerConnector> logger) : IIdentityServerConnector
    {
        private const string AccountConfirmationApiUrl = "api/identity/account/{0}/confirmation";
        private readonly IdentityServerConfiguration _configuration = configuration.GetIdentityServerConfiguration();

        async Task<string> IIdentityServerConnector.GetAccountConfirmationUrl(string userId)
        {
            var endpoint = string.Format(AccountConfirmationApiUrl, userId);

            using var client = CreateClient();
            using var request = new HttpRequestMessage(HttpMethod.Post, endpoint);

            try
            {
                logger.LogInformation("Fetching confirmaion link from identity server from {url} for user {userId}",
                    endpoint, userId);

                var response = await client.SendAsync(request);
                var confirmationUrl = await response.Content.ReadAsStringAsync();

                Guard.IsNotNullOrWhiteSpace(confirmationUrl);

                return confirmationUrl!;
            }
            catch (Exception e)
            {
                logger.LogError(e, "Something went wrong while fetching confirmation link for user {0}", userId);

                throw;
            }
        }

        private HttpClient CreateClient()
        {
            var client = httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration.BaseUrl);
            client.Timeout = _configuration.Timeout;
            client.DefaultRequestHeaders.Add("x-api-key", _configuration.ApiKey);

            return client;
        }
    }
}
