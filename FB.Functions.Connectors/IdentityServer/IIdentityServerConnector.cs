namespace FB.Functions.Connectors.IdentityServer
{
    public interface IIdentityServerConnector
    {
        Task<string> GetAccountConfirmationUrl(string userId);
    }
}
