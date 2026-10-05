using System.ComponentModel.DataAnnotations;

namespace Piu.Api.Features.Auth.FazerLogin;

public record FazerLoginRequest(
    [Required(ErrorMessage = Mensagens.UsuarioObrigatorio)]
    string Usuario,

    [Required(ErrorMessage = Mensagens.SenhaObrigatoria)]
    string Senha
);
