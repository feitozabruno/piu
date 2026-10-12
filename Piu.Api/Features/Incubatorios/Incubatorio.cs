using Piu.Api.Domain;

namespace Piu.Api.Features.Incubatorios;

public class Incubatorio
{
    public string Identificador { get; private set; }
    public string Nome { get; private set; }
    public string Sigla { get; private set; }
    public bool Ativo { get; private set; }
    public DateTime DataCadastro { get; private set; }

    private Incubatorio()
    {
        Identificador = string.Empty;
        Sigla = string.Empty;
        Nome = string.Empty;
    }

    private Incubatorio(string identificador, string nome, string sigla)
    {
        Identificador = identificador;
        Nome = nome;
        Sigla = sigla;
        Ativo = true;
        DataCadastro = DateTime.UtcNow;
    }

    public static Incubatorio Criar(string identificador, string nome, string sigla)
    {
        identificador = identificador?.Trim().ToUpperInvariant() ?? string.Empty;
        nome = nome?.Trim() ?? string.Empty;
        sigla = sigla?.Trim().ToUpperInvariant() ?? string.Empty;

        if (identificador.Length == 0) throw new DomainException(IncubatorioErros.IdentificadorObrigatorio);
        if (identificador.Length < IncubatorioLimites.IdentificadorMin) throw new DomainException(IncubatorioErros.IdentificadorCurto);
        if (identificador.Length > IncubatorioLimites.IdentificadorMax) throw new DomainException(IncubatorioErros.IdentificadorMuitoGrande);

        if (nome.Length == 0) throw new DomainException(IncubatorioErros.NomeObrigatorio);
        if (nome.Length < IncubatorioLimites.NomeMin) throw new DomainException(IncubatorioErros.NomeCurto);
        if (nome.Length > IncubatorioLimites.NomeMax) throw new DomainException(IncubatorioErros.NomeMuitoGrande);

        if (sigla.Length == 0) throw new DomainException(IncubatorioErros.SiglaObrigatoria);
        if (sigla.Length < IncubatorioLimites.SiglaMin) throw new DomainException(IncubatorioErros.SiglaCurta);
        if (sigla.Length > IncubatorioLimites.SiglaMax) throw new DomainException(IncubatorioErros.SiglaMuitoGrande);

        return new Incubatorio(identificador, nome, sigla);
    }
}

public static class IncubatorioLimites
{
    public const int IdentificadorMin = 3;
    public const int IdentificadorMax = 50;
    public const int NomeMin = 3;
    public const int NomeMax = 150;
    public const int SiglaMin = 3;
    public const int SiglaMax = 20;
}
