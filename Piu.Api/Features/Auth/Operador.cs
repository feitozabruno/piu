using Microsoft.AspNetCore.Identity;

namespace Piu.Api.Features.Auth;

public sealed class Operador : IdentityUser
{
    public string Nome { get; private set; }
    public string Cargo { get; private set; }
    public string Incubatorio { get; private set; }
    public bool Ativo { get; private set; }

    private Operador()
    {
        Nome = string.Empty;
        Cargo = string.Empty;
        Incubatorio = string.Empty;
    }

    public Operador(string username, string nome, string cargo, string incubatorio)
    {
        UserName = username;
        Nome = nome;
        Cargo = cargo;
        Incubatorio = incubatorio;
        Ativo = true;
    }

    public void Desativar() => Ativo = false;
    public void Reativar() => Ativo = true;
}
