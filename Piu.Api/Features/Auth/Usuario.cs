using Microsoft.AspNetCore.Identity;
using Piu.Api.Exceptions;

namespace Piu.Api.Features.Auth;

public sealed class Usuario : IdentityUser
{
    public const string Administrador = "Administrador";
    public const string Gerente = "Gerente";
    public const string Funcionario = "Funcionario";

    public string Nome { get; private set; }
    public string Cargo { get; private set; }
    public string? Incubatorio { get; private set; }
    public string Perfil { get; private set; }
    public bool Ativo { get; private set; }

    private Usuario()
    {
        Nome = string.Empty;
        Cargo = string.Empty;
        Incubatorio = null;
        Perfil = Funcionario;
    }

    private Usuario(string usuario, string nome, string cargo, string? incubatorio, string perfil)
    {
        UserName = usuario;
        Nome = nome;
        Cargo = cargo;
        Incubatorio = incubatorio;
        Perfil = perfil;
        Ativo = true;
    }

    public static Usuario Criar(
        string usuario,
        string nome,
        string cargo,
        string? incubatorio,
        string perfil = Funcionario)
    {
        usuario = usuario?.Trim().ToLowerInvariant() ?? string.Empty;
        nome = nome?.Trim() ?? string.Empty;
        cargo = cargo?.Trim() ?? string.Empty;
        perfil = perfil?.Trim() ?? Funcionario;
        incubatorio = perfil == Administrador
            ? null
            : incubatorio?.Trim().ToUpperInvariant() ?? string.Empty;

        if (usuario.Length == 0) throw new DomainException(UsuarioErros.UsuarioObrigatorio);
        if (usuario.Length < UsuarioLimites.UsuarioMin) throw new DomainException(UsuarioErros.UsuarioCurto);
        if (usuario.Length > UsuarioLimites.UsuarioMax) throw new DomainException(UsuarioErros.UsuarioMuitoGrande);

        if (nome.Length == 0) throw new DomainException(UsuarioErros.NomeObrigatorio);
        if (nome.Length < UsuarioLimites.NomeMin) throw new DomainException(UsuarioErros.NomeCurto);
        if (nome.Length > UsuarioLimites.NomeMax) throw new DomainException(UsuarioErros.NomeMuitoGrande);

        if (cargo.Length == 0) throw new DomainException(UsuarioErros.CargoObrigatorio);
        if (cargo.Length < UsuarioLimites.CargoMin) throw new DomainException(UsuarioErros.CargoCurto);
        if (cargo.Length > UsuarioLimites.CargoMax) throw new DomainException(UsuarioErros.CargoMuitoGrande);

        if (perfil != Administrador)
        {
            if (incubatorio!.Length == 0) throw new DomainException(UsuarioErros.IncubatorioObrigatorio);
            if (incubatorio.Length < UsuarioLimites.IncubatorioMin) throw new DomainException(UsuarioErros.IncubatorioCurto);
            if (incubatorio.Length > UsuarioLimites.IncubatorioMax) throw new DomainException(UsuarioErros.IncubatorioMuitoGrande);
        }

        if (perfil is not (Administrador or Gerente or Funcionario))
            throw new DomainException(UsuarioErros.PerfilInvalido);

        return new Usuario(usuario, nome, cargo, incubatorio, perfil);
    }
}

public static class UsuarioLimites
{
    public const int UsuarioMin = 3;
    public const int UsuarioMax = 50;
    public const int SenhaMin = 6;
    public const int SenhaMax = 30;
    public const int NomeMin = 3;
    public const int NomeMax = 200;
    public const int CargoMin = 2;
    public const int CargoMax = 50;
    public const int IncubatorioMin = 3;
    public const int IncubatorioMax = 50;
}
