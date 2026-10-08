using Microsoft.AspNetCore.Identity;
using Piu.Api.Application;

namespace Piu.Api.Features.Auth.CadastrarUsuario;

public class CadastrarUsuarioHandler(UserManager<Usuario> userManager)
{
    public async Task<Usuario> HandleAsync(CadastrarUsuarioRequest request)
    {
        var usuario = Usuario.Criar(
            usuario: request.Usuario.Trim(),
            nome: request.Nome.Trim(),
            cargo: request.Cargo.Trim(),
            incubatorio: request.Incubatorio.Trim()
        );

        var resultado = await userManager.CreateAsync(user: usuario, password: request.Senha);

        if (resultado.Succeeded) return usuario;

        foreach (var erro in resultado.Errors)
        {
            switch (erro.Code)
            {
                case "DuplicateUserName":
                    throw new ApplicationErrorException(CadastrarUsuarioErros.UsuarioEmUso);
                case "PasswordTooShort":
                case "PasswordRequiresDigit":
                case "PasswordRequiresUpper":
                case "PasswordRequiresLower":
                case "PasswordRequiresNonAlphanumeric":
                    throw new ApplicationErrorException(CadastrarUsuarioErros.SenhaInvalida);
            }
        }

        throw new ApplicationErrorException(
            CadastrarUsuarioErros.FalhaAoCadastrar);
    }
}
