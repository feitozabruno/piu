namespace Piu.Api.Features.Lotes.ListarLotes;

public sealed record ListarLotesResponse(
    Guid Id,
    string Nome,
    DateOnly DataNascimento,
    bool Ativo
);
