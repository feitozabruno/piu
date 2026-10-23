using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Piu.Api.Exceptions;
using Piu.Api.Infrastructure.Persistence.Configurations;

namespace Piu.Api.Features.Auth.CadastrarUsuario;

public class CadastrarUsuarioHandler(UserManager<Usuario> userManager)
{
    public async Task<Usuario> HandleAsync(CadastrarUsuarioRequest request)
    {
        var usuario = Usuario.Criar(
            usuario: request.Usuario,
            nome: request.Nome,
            cargo: request.Cargo,
            incubatorio: request.Incubatorio);

        IdentityResult resultado;

        try
        {
            resultado = await userManager.CreateAsync(usuario, request.Senha);
        }
        catch (DbUpdateException ex) when (
            ex.InnerException is PostgresException
            {
                SqlState: PostgresErrorCodes.ForeignKeyViolation,
                ConstraintName: UsuarioConstraints.FkIncubatorio
            })
        {
            throw new ApplicationErrorException(CadastrarUsuarioErros.ErroIncubatorioInvalido);
        }

        if (resultado.Succeeded) return usuario;

        foreach (var erro in resultado.Errors)
        {
            switch (erro.Code)
            {
                case "DuplicateUserName":
                    throw new ApplicationErrorException(CadastrarUsuarioErros.ErroUsuarioEmUso);
                case "PasswordTooShort":
                case "PasswordRequiresDigit":
                case "PasswordRequiresUpper":
                case "PasswordRequiresLower":
                case "PasswordRequiresNonAlphanumeric":
                    throw new ApplicationErrorException(CadastrarUsuarioErros.ErroSenhaInvalida);
            }
        }

        throw new ApplicationErrorException(CadastrarUsuarioErros.FalhaAoCadastrar);
    }
}
