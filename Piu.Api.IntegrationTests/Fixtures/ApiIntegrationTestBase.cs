namespace Piu.Api.IntegrationTests.Fixtures;

public abstract class ApiIntegrationTestBase(
    CustomWebApplicationFactory factory) : IAsyncLifetime
{
    protected CustomWebApplicationFactory Factory { get; } = factory;
    protected HttpClient Http { get; } = factory.CreateClient();

    public Task InitializeAsync() => Factory.ResetDatabaseAsync();
    public Task DisposeAsync() => Task.CompletedTask;
}
