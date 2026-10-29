namespace Piu.Api.Tests.Integration.Fixtures;

public sealed record ApiValidationErrorResponse(
    string Title,
    int Status,
    Dictionary<string, string[]> Errors
);
