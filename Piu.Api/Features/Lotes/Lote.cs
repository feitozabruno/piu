using Piu.Api.Exceptions;
using Piu.Api.Features.Incubatorios;

namespace Piu.Api.Features.Lotes;

public sealed class Lote
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Nome { get; private set; } = string.Empty;
    public DateOnly DataNascimento { get; private set; }
    public bool Ativo { get; private set; } = true;
    public Incubatorio Incubatorio { get; private set; } = null!;
    public string IncubatorioIdentificador { get; private set; } = string.Empty;

    private Lote() { }

    private Lote(string nome, DateOnly dataNascimento, string incubatorioIdentificador)
    {
        Nome = nome;
        DataNascimento = dataNascimento;
        IncubatorioIdentificador = incubatorioIdentificador;
    }

    public static Lote Criar(string nome, DateOnly dataNascimento, string incubatorioIdentificador)
    {
        nome = nome?.Trim() ?? string.Empty;
        incubatorioIdentificador = incubatorioIdentificador?.Trim().ToUpperInvariant() ?? string.Empty;

        ValidarDados(nome, dataNascimento);

        if (incubatorioIdentificador.Length == 0)
            throw new DomainException(LoteErros.IncubatorioObrigatorio);

        return new Lote(nome.ToUpperInvariant(), dataNascimento, incubatorioIdentificador);
    }

    public void Atualizar(string nome, DateOnly dataNascimento, bool ativo)
    {
        nome = nome?.Trim() ?? string.Empty;
        ValidarDados(nome, dataNascimento);

        Nome = nome.ToUpperInvariant();
        DataNascimento = dataNascimento;
        Ativo = ativo;
    }

    public string CalcularIdadeEm(DateOnly data)
    {
        if (data < DataNascimento)
            throw new DomainException(LoteErros.DataReferenciaInvalida);

        var totalDias = data.DayNumber - DataNascimento.DayNumber;
        var semanas = totalDias / 7;
        var dias = totalDias % 7;

        return $"{semanas},{dias}";
    }

    private static void ValidarDados(string nome, DateOnly dataNascimento)
    {
        if (nome.Length == 0) throw new DomainException(LoteErros.NomeObrigatorio);
        if (nome.Length < LoteLimites.NomeMin) throw new DomainException(LoteErros.NomeCurto);
        if (nome.Length > LoteLimites.NomeMax) throw new DomainException(LoteErros.NomeMuitoGrande);
        if (dataNascimento == default) throw new DomainException(LoteErros.DataNascimentoObrigatoria);

        if (dataNascimento > DateOnly.FromDateTime(DateTime.UtcNow))
            throw new DomainException(LoteErros.DataNascimentoInvalida);
    }
}

public static class LoteLimites
{
    public const int NomeMin = 3;
    public const int NomeMax = 30;
}
