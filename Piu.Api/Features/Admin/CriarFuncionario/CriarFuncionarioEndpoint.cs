using Microsoft.AspNetCore.Identity;
using Piu.Api.Exceptions;
using Piu.Api.Features.Auth;

namespace Piu.Api.Features.Admin.CriarFuncionario;

public static class CriarFuncionarioEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/funcionarios", async (
                CriarFuncionarioRequest request,
                UserManager<Usuario> userManager,
                HttpContext httpContext) =>
            {
                var incubatorio = httpContext.User.FindFirst("incubatorio")?.Value;
                if (string.IsNullOrWhiteSpace(incubatorio))
                    throw new ApplicationErrorException(AdminErrors.IncubatorioNaoEncontrado);

                var funcionario = Usuario.Criar(
                    request.Usuario, request.Nome, request.Cargo, incubatorio, Usuario.Funcionario);
                var resultado = await userManager.CreateAsync(funcionario, request.Senha);

                if (!resultado.Succeeded)
                {
                    if (resultado.Errors.Any(e => e.Code == "DuplicateUserName"))
                        throw new ApplicationErrorException(AdminErrors.UsuarioEmUso);

                    throw new ApplicationErrorException(AdminErrors.FalhaAoCriarUsuario);
                }

                return Results.Created(
                    $"/api/v1/usuarios/{funcionario.Id}",
                    new { funcionario.Id, funcionario.UserName, funcionario.Nome, funcionario.Cargo, funcionario.Incubatorio });
            })
            .RequireAuthorization(Usuario.Gerente)
            .WithName("CriarFuncionario");
    }
}
