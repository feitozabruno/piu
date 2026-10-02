using System.ComponentModel.DataAnnotations;

namespace Piu.Api.Features.Auth.CadastrarOperador;

public record CadastrarOperadorRequest(
    [Required(ErrorMessage = Mensagens.NomeUsuarioObrigatorio)]
    [MinLength(3, ErrorMessage = Mensagens.NomeUsuarioCurto)]
    [MaxLength(30, ErrorMessage = Mensagens.NomeUsuarioMuitoGrande)]
    string Usuario,

    [Required(ErrorMessage = Mensagens.SenhaObrigatoria)]
    [MinLength(6, ErrorMessage = Mensagens.SenhaFraca)]
    [MaxLength(30, ErrorMessage = Mensagens.SenhaMuitoGrande)]
    string Senha,

    [Required(ErrorMessage = Mensagens.NomeObrigatorio)]
    [MinLength(3, ErrorMessage = Mensagens.NomeCurto)]
    [MaxLength(200, ErrorMessage = Mensagens.NomeMuitoGrande)]
    string Nome,

    [Required(ErrorMessage = Mensagens.CargoObrigatorio)]
    [MinLength(2, ErrorMessage = Mensagens.CargoCurto)]
    [MaxLength(50, ErrorMessage = Mensagens.CargoMuitoGrande)]
    string Cargo
);
