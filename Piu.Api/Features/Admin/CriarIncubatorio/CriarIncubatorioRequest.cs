using System.ComponentModel.DataAnnotations;
using Piu.Api.Features.Auth;
using Piu.Api.Features.Incubatorios;

namespace Piu.Api.Features.Admin.CriarIncubatorio;

public sealed record CriarIncubatorioRequest(
    [Required, MinLength(IncubatorioLimites.IdentificadorMin), MaxLength(IncubatorioLimites.IdentificadorMax)]
    string Identificador,
    [Required, MinLength(IncubatorioLimites.NomeMin), MaxLength(IncubatorioLimites.NomeMax)]
    string Nome,
    [Required, MinLength(IncubatorioLimites.SiglaMin), MaxLength(IncubatorioLimites.SiglaMax)]
    string Sigla,
    [Required, MinLength(UsuarioLimites.UsuarioMin), MaxLength(UsuarioLimites.UsuarioMax)]
    string UsuarioGerente,
    [Required, MinLength(UsuarioLimites.SenhaMin), MaxLength(UsuarioLimites.SenhaMax)]
    string SenhaGerente,
    [Required, MinLength(UsuarioLimites.NomeMin), MaxLength(UsuarioLimites.NomeMax)]
    string NomeGerente
);
