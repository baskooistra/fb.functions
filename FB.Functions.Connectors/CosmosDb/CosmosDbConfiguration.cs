namespace FB.Functions.Connectors.CosmosDb
{
    public class CosmosDbConfiguration
    {
        public required string DatabaseName { get; set; }
        public required string ContainerName { get; set; }
    }
}
