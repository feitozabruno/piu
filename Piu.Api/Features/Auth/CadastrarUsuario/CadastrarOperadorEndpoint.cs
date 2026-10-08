namespace Piu.Api.Features.Auth.CadastrarUsuario;

public static class CadastrarUsuarioEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/cadastrar", CriarAsync);
    }

    private static async Task<IResult> CriarAsync(
        CadastrarUsuarioRequest request,
        CadastrarUsuarioHandler handler)
    {
        var novoUsuario = await handler.HandleAsync(request);

        var response = new CadastrarUsuarioResponse(
            Id: novoUsuario.Id,
            Usuario: novoUsuario.UserName!,
            Nome: novoUsuario.Nome,
            Cargo: novoUsuario.Cargo,
            Incubatorio: novoUsuario.Incubatorio
        );

        return Results.Created($"/api/v1/usuarios/{novoUsuario.Id}", response);
    }
};
