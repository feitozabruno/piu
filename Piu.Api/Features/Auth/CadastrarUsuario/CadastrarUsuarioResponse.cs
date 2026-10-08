namespace Piu.Api.Features.Auth.CadastrarUsuario;

public sealed record CadastrarUsuarioResponse(
    string Id,
    string Usuario,
    string Nome,
    string Cargo,
    string Incubatorio
);
