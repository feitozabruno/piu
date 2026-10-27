using System.ComponentModel.DataAnnotations;

namespace Piu.Api.Features.Lotes.AtualizarLote;

public record AtualizarLoteRequest(
    [Required(ErrorMessage = AtualizarLoteErros.NomeObrigatorio)]
    [MinLength(LoteLimites.NomeMin, ErrorMessage = AtualizarLoteErros.NomeCurto)]
    [MaxLength(LoteLimites.NomeMax, ErrorMessage = AtualizarLoteErros.NomeMuitoGrande)]
    string Nome,

    [Required(ErrorMessage = AtualizarLoteErros.DataNascimentoObrigatoria)]
    string? DataNascimento,

    bool Ativo
);
