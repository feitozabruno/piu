using System.ComponentModel.DataAnnotations;

namespace Piu.Api.Features.Auth.CadastrarUsuario;

public record CadastrarUsuarioRequest(
    [Required(ErrorMessage = CadastrarUsuarioErros.NomeUsuarioObrigatorio)]
    [MinLength(UsuarioLimites.UsuarioMin, ErrorMessage = CadastrarUsuarioErros.NomeUsuarioCurto)]
    [MaxLength(UsuarioLimites.UsuarioMax, ErrorMessage = CadastrarUsuarioErros.NomeUsuarioMuitoGrande)]
    string Usuario,

    [Required(ErrorMessage = CadastrarUsuarioErros.SenhaObrigatoria)]
    [MinLength(UsuarioLimites.SenhaMin, ErrorMessage = CadastrarUsuarioErros.SenhaFraca)]
    [MaxLength(UsuarioLimites.SenhaMax, ErrorMessage = CadastrarUsuarioErros.SenhaMuitoGrande)]
    string Senha,

    [Required(ErrorMessage = CadastrarUsuarioErros.NomeObrigatorio)]
    [MinLength(UsuarioLimites.NomeMin, ErrorMessage = CadastrarUsuarioErros.NomeCurto)]
    [MaxLength(UsuarioLimites.NomeMax, ErrorMessage = CadastrarUsuarioErros.NomeMuitoGrande)]
    string Nome,

    [Required(ErrorMessage = CadastrarUsuarioErros.CargoObrigatorio)]
    [MinLength(UsuarioLimites.CargoMin, ErrorMessage = CadastrarUsuarioErros.CargoCurto)]
    [MaxLength(UsuarioLimites.CargoMax, ErrorMessage = CadastrarUsuarioErros.CargoMuitoGrande)]
    string Cargo,

    [Required(ErrorMessage = CadastrarUsuarioErros.NomeIncubatorioObrigatorio)]
    [MinLength(UsuarioLimites.IncubatorioMin, ErrorMessage = CadastrarUsuarioErros.NomeIncubatorioCurto)]
    [MaxLength(UsuarioLimites.IncubatorioMax, ErrorMessage = CadastrarUsuarioErros.NomeIncubatorioMuitoGrande)]
    string Incubatorio
);
