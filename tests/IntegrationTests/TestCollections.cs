using Xunit;

namespace IntegrationTests;

public static class TestCollections
{
    [CollectionDefinition("Dancer repository collection")]
    public class DancerRepositoryTestCollection : ICollectionFixture<PostgresDatabaseFixture>
    {
    }

    [CollectionDefinition("Dancer service collection")]
    public class DancerServiceTestCollection : ICollectionFixture<PostgresDatabaseFixture>
    {
    }

    [CollectionDefinition("Event repository collection")]
    public class EventRepositoryTestCollection : ICollectionFixture<PostgresDatabaseFixture>
    {
    }

    [CollectionDefinition("Song repository collection")]
    public class SongRepositoryTestCollection : ICollectionFixture<PostgresDatabaseFixture>
    {
    }
}