using Xunit;
[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace aus_ddr_api.IntegrationTests
{
    [CollectionDefinition("Postgres database collection")]
    public class PostgresDatabaseCollection : ICollectionFixture<PostgresDatabaseFixture>
    {
        
    }
}