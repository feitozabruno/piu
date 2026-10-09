using System.ComponentModel.DataAnnotations;

namespace Piu.Api.Features.Auth.FazerLogin;

public record FazerLoginRequest(
    [Required(ErrorMessage = FazerLoginErros.UsuarioObrigatorio)]
    string Usuario,

    [Required(ErrorMessage = FazerLoginErros.SenhaObrigatoria)]
    string Senha
);
