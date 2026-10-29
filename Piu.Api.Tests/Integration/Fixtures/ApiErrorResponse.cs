namespace Piu.Api.Tests.Integration.Fixtures;

public sealed record ApiErrorResponse(
    string Code,
    string Detail,
    int Status
);
