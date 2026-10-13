namespace Piu.Api.IntegrationTests.Fixtures;

public sealed record ApiErrorResponse(
    string Code,
    string Detail,
    int Status
);
