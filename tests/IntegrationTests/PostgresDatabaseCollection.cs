using Xunit;

namespace IntegrationTests
{
    [CollectionDefinition("Postgres database collection")]
    public class PostgresDatabaseCollection : ICollectionFixture<PostgresDatabaseFixture>
    {
        
    }
}