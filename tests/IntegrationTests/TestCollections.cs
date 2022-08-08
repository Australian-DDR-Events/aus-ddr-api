using Xunit;

namespace IntegrationTests;

public static class TestCollections
{
    [CollectionDefinition("Badge repository collection")]
    public class BadgeRepositoryTestCollection : ICollectionFixture<PostgresDatabaseFixture>
    {
    }
    
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

    [CollectionDefinition("Reward repository collection")]
    public class RewardRepositoryTestCollection : ICollectionFixture<PostgresDatabaseFixture>
    {
    }

    [CollectionDefinition("Reward quality repository collection")]
    public class RewardQualityRepositoryTestCollection : ICollectionFixture<PostgresDatabaseFixture>
    {
    }
}