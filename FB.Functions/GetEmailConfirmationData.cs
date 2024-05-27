using System.Net;
using CommunityToolkit.Diagnostics;
using FB.Functions.Connectors.IdentityServer;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace FB.Functions;

public class GetEmailConfirmationData(ILoggerFactory loggerFactory, IIdentityServerConnector identityServerConnector)
{
    private readonly ILogger _logger = loggerFactory.CreateLogger<GetEmailConfirmationData>();

    [Function("GetEmailConfirmationData")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req, CancellationToken cancellationToken)
    {
        var body = await new StreamReader(req.Body).ReadToEndAsync();
        _logger.LogInformation("C# HTTP trigger function processed a request. Body: {body}", body);

        try
        {
            var requestData = JObject.Parse(body) ?? throw new ArgumentNullException(body, "Invalid request body for GetEmailConfirmationData");
            var userId = (string)requestData["userId"]!;
            Guard.IsNotNullOrWhiteSpace(userId);

            var confirmationLink = await identityServerConnector.GetAccountConfirmationUrl(userId, cancellationToken);

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(confirmationLink, cancellationToken);

            return response;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to fetch confirmation link from identity server");
            throw;
        }
    }
}