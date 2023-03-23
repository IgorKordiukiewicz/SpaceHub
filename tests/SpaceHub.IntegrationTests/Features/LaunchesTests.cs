using Xunit;

namespace SpaceHub.IntegrationTests.Features;

[Collection(nameof(IntegrationTestsCollection))]
public class LaunchesTests
{
    private readonly IntegrationTestsFixture _fixture;

    public LaunchesTests(IntegrationTestsFixture fixture)
    {
        _fixture = fixture;

        _fixture.InitDb();
    }
}
