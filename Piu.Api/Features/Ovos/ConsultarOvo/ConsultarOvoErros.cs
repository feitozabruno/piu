using Piu.Api.Exceptions;

namespace Piu.Api.Features.Ovos.ConsultarOvo;

public static class ConsultarOvoErros
{
    public static readonly ApplicationError ErroNaoEncontrado =
        new("OVO_NAO_ENCONTRADO", "O ovo informado não foi encontrado.", StatusCodes.Status404NotFound);
}
