using System.ComponentModel.DataAnnotations;
using Piu.Api.Features.Auth;

namespace Piu.Api.Features.Admin.CriarFuncionario;

public sealed record CriarFuncionarioRequest(
    [Required, MinLength(UsuarioLimites.UsuarioMin), MaxLength(UsuarioLimites.UsuarioMax)]
    string Usuario,
    [Required, MinLength(UsuarioLimites.SenhaMin), MaxLength(UsuarioLimites.SenhaMax)]
    string Senha,
    [Required, MinLength(UsuarioLimites.NomeMin), MaxLength(UsuarioLimites.NomeMax)]
    string Nome,
    [Required, MinLength(UsuarioLimites.CargoMin), MaxLength(UsuarioLimites.CargoMax)]
    string Cargo);
