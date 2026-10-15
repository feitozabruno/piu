using Piu.Api.Domain;

namespace Piu.Api.Features.Lotes;

public sealed class Lote
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public DateOnly DataNascimento { get; private set; }

    private Lote() { }

    private Lote(string nome, DateOnly dataNascimento)
    {
        Id = Guid.NewGuid();
        Nome = nome;
        DataNascimento = dataNascimento;
    }

    public static Lote Criar(string nome, DateOnly dataNascimento)
    {
        nome = nome?.Trim() ?? string.Empty;

        if (nome.Length == 0) throw new DomainException(LoteErros.NomeObrigatorio);
        if (nome.Length < LoteLimites.NomeMin) throw new DomainException(LoteErros.NomeCurto);
        if (nome.Length > LoteLimites.NomeMax) throw new DomainException(LoteErros.NomeMuitoGrande);

        if (dataNascimento == default) throw new DomainException(LoteErros.DataNascimentoObrigatoria);

        if (dataNascimento > DateOnly.FromDateTime(DateTime.UtcNow))
        {
            throw new DomainException(LoteErros.DataNascimentoInvalida);
        }

        return new Lote(nome.ToUpperInvariant(), dataNascimento);
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
}

public static class LoteLimites
{
    public const int NomeMin = 3;
    public const int NomeMax = 30;
}
