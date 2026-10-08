using Microsoft.AspNetCore.Identity;
using Piu.Api.Domain;

namespace Piu.Api.Features.Auth;

public sealed class Usuario : IdentityUser
{
    public string Nome { get; private set; }
    public string Cargo { get; private set; }
    public string Incubatorio { get; private set; }
    public bool Ativo { get; private set; }

    private Usuario()
    {
        Nome = string.Empty;
        Cargo = string.Empty;
        Incubatorio = string.Empty;
    }

    private Usuario(string usuario, string nome, string cargo, string incubatorio)
    {
        UserName = usuario;
        Nome = nome;
        Cargo = cargo;
        Incubatorio = incubatorio;
        Ativo = true;
    }

    public static Usuario Criar(string usuario, string nome, string cargo, string incubatorio)
    {
        if (string.IsNullOrWhiteSpace(usuario)) throw new DomainException(ErrosUsuario.UsuarioObrigatorio);
        if (string.IsNullOrWhiteSpace(nome)) throw new DomainException(ErrosUsuario.NomeObrigatorio);
        if (string.IsNullOrWhiteSpace(cargo)) throw new DomainException(ErrosUsuario.CargoObrigatorio);
        if (string.IsNullOrWhiteSpace(incubatorio)) throw new DomainException(ErrosUsuario.IncubatorioObrigatorio);

        return new Usuario(usuario, nome, cargo, incubatorio);
    }
}
