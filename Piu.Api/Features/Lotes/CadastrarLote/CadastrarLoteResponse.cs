namespace Piu.Api.Features.Lotes.CadastrarLote;

public sealed record CadastrarLoteResponse(
    Guid Id,
    string Nome,
    DateOnly DataNascimento
);
