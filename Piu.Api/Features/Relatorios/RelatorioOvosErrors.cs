using Piu.Api.Exceptions;

namespace Piu.Api.Features.Relatorios;

public static class RelatorioOvosErrors
{
    public static readonly ApplicationError IntervaloInvalido =
        new("INTERVALO_RELATORIO_INVALIDO", "A data inicial deve ser anterior ou igual à data final.");

    public static readonly ApplicationError LimiteRegistrosExcedido =
        new("LIMITE_REGISTROS_RELATORIO_EXCEDIDO",
            "A quantidade de registros do dia excede a capacidade do formulário.");

    public static readonly ApplicationError FormularioIndisponivel =
        new("FORMULARIO_RELATORIO_INDISPONIVEL",
            "O formulário do relatório não está disponível.");
}
