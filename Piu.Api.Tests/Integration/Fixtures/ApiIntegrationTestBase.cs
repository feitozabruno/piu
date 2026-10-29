namespace Piu.Api.Tests.Integration.Fixtures;

public abstract class ApiIntegrationTestBase : IAsyncLifetime
{
    private readonly CustomWebApplicationFactory _factory;
    protected readonly HttpClient ApiClient;
    protected readonly AuthHelper Auth;

    public ApiIntegrationTestBase(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        ApiClient = factory.CreateClient();
        Auth = new AuthHelper(ApiClient);
    }

    public Task InitializeAsync() => _factory.ResetDatabaseAsync();
    public Task DisposeAsync() => Task.CompletedTask;
}
