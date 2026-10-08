using System.ComponentModel.DataAnnotations;

namespace Piu.Api.Features.Auth.CadastrarUsuario;

public record CadastrarUsuarioRequest(
    [Required(ErrorMessage = CadastrarUsuarioErros.NomeUsuarioObrigatorio)]
    [MinLength(3, ErrorMessage = CadastrarUsuarioErros.NomeUsuarioCurto)]
    [MaxLength(30, ErrorMessage = CadastrarUsuarioErros.NomeUsuarioMuitoGrande)]
    string Usuario,

    [Required(ErrorMessage = CadastrarUsuarioErros.SenhaObrigatoria)]
    [MinLength(6, ErrorMessage = CadastrarUsuarioErros.SenhaFraca)]
    [MaxLength(30, ErrorMessage = CadastrarUsuarioErros.SenhaMuitoGrande)]
    string Senha,

    [Required(ErrorMessage = CadastrarUsuarioErros.NomeObrigatorio)]
    [MinLength(3, ErrorMessage = CadastrarUsuarioErros.NomeCurto)]
    [MaxLength(200, ErrorMessage = CadastrarUsuarioErros.NomeMuitoGrande)]
    string Nome,

    [Required(ErrorMessage = CadastrarUsuarioErros.CargoObrigatorio)]
    [MinLength(2, ErrorMessage = CadastrarUsuarioErros.CargoCurto)]
    [MaxLength(50, ErrorMessage = CadastrarUsuarioErros.CargoMuitoGrande)]
    string Cargo,

    [Required(ErrorMessage = CadastrarUsuarioErros.NomeIncubatorioObrigatorio)]
    [MinLength(2, ErrorMessage = CadastrarUsuarioErros.NomeIncubatorioCurto)]
    [MaxLength(50, ErrorMessage = CadastrarUsuarioErros.NomeIncubatorioMuitoGrande)]
    string Incubatorio
);
