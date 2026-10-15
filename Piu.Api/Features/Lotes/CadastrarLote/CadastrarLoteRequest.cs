using System.ComponentModel.DataAnnotations;

namespace Piu.Api.Features.Lotes.CadastrarLote;

public record CadastrarLoteRequest(
    [Required(ErrorMessage = CadastrarLoteErros.NomeObrigatorio)]
    [MinLength(LoteLimites.NomeMin, ErrorMessage = CadastrarLoteErros.NomeCurto)]
    [MaxLength(LoteLimites.NomeMax, ErrorMessage = CadastrarLoteErros.NomeMuitoGrande)]
    string Nome,

    [Required(ErrorMessage = CadastrarLoteErros.DataNascimentoObrigatoria)]
    string? DataNascimento
);
