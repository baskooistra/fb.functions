using CommunityToolkit.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace FB.Functions.Connectors.IdentityServer
{
    public class IdentityServerConfiguration
    {
        public const string Name = "IdentityServer";

        public required string BaseUrl { get; set; }
        public required TimeSpan Timeout { get; set; }
        public required string ApiKey { get; set; }
    }

    public static class IConfigurationExtension
    {
        public static IdentityServerConfiguration GetIdentityServerConfiguration(this IConfiguration configuration)
        {
            var section = configuration.GetSection(IdentityServerConfiguration.Name);
            var identityServerConfiguration = section.Get<IdentityServerConfiguration>();

            Guard.IsNotNull(identityServerConfiguration, nameof(identityServerConfiguration));

            return identityServerConfiguration;
        }
    }
}
