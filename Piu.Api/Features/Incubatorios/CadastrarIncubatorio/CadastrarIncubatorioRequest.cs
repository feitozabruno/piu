using System.ComponentModel.DataAnnotations;

namespace Piu.Api.Features.Incubatorios.CadastrarIncubatorio;

public record CadastrarIncubatorioRequest(
    [Required(ErrorMessage = CadastrarIncubatorioErros.IdentificadorObrigatorio)]
    [MinLength(IncubatorioLimites.IdentificadorMin, ErrorMessage = CadastrarIncubatorioErros.IdentificadorCurto)]
    [MaxLength(IncubatorioLimites.IdentificadorMax, ErrorMessage = CadastrarIncubatorioErros.IdentificadorMuitoGrande)]
    string Identificador,

    [Required(ErrorMessage = CadastrarIncubatorioErros.NomeObrigatorio)]
    [MinLength(IncubatorioLimites.NomeMin, ErrorMessage = CadastrarIncubatorioErros.NomeCurto)]
    [MaxLength(IncubatorioLimites.NomeMax, ErrorMessage = CadastrarIncubatorioErros.NomeMuitoGrande)]
    string Nome,

    [Required(ErrorMessage = CadastrarIncubatorioErros.SiglaObrigatoria)]
    [MinLength(IncubatorioLimites.SiglaMin, ErrorMessage = CadastrarIncubatorioErros.SiglaCurta)]
    [MaxLength(IncubatorioLimites.SiglaMax, ErrorMessage = CadastrarIncubatorioErros.SiglaMuitoGrande)]
    string Sigla
);
