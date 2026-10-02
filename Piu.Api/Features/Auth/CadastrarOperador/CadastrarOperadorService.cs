using Microsoft.AspNetCore.Identity;

namespace Piu.Api.Features.Auth.CadastrarOperador;

public class CadastrarOperadorService(UserManager<Operador> userManager)
{
    public async Task<CadastrarOperadorResponse> CriarAsync(CadastrarOperadorRequest request)
    {
        var operador = new Operador(
            username: request.Usuario,
            nome: request.Nome,
            cargo: request.Cargo
        );

        var resultado = await userManager.CreateAsync(
            user: operador,
            password: request.Senha
        );

        if (resultado.Succeeded)
            return CadastrarOperadorResponse.Criado(operador.Id);

        var nomeUsuarioDuplicado = resultado.Errors
            .Any(e => e.Code == nameof(IdentityErrorDescriber.DuplicateUserName));

        if (nomeUsuarioDuplicado)
            return CadastrarOperadorResponse.Falha(nameof(request.Usuario), Mensagens.NomeUsuarioEmUso);

        var erros = resultado.Errors
            .GroupBy(e => CampoDoErro(e.Code))
            .ToDictionary(g => g.Key, g => g.Select(e => e.Description).ToArray());

        return CadastrarOperadorResponse.Falha(erros);
    }

    private static string CampoDoErro(string codigo) => codigo switch
    {
        _ when codigo.StartsWith("Password") => nameof(CadastrarOperadorRequest.Senha),
        _ when codigo.Contains("UserName") => nameof(CadastrarOperadorRequest.Usuario),
        _ => string.Empty
    };
}
