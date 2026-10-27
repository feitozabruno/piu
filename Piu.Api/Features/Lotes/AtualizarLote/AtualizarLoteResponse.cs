namespace Piu.Api.Features.Lotes.AtualizarLote;

public sealed record AtualizarLoteResponse(
    Guid Id,
    string Nome,
    DateOnly DataNascimento,
    bool Ativo
);
