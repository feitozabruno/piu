namespace Piu.Api.IntegrationTests.Fixtures;

public sealed record ApiValidationErrorResponse(
    string Title,
    int Status,
    Dictionary<string, string[]> Errors
);
