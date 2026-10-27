using PdfSharp.Fonts;
using Piu.Api.Extensions;
using Piu.Api.Features.Relatorios;
using Piu.Api.Infrastructure.Persistence;

GlobalFontSettings.FontResolver = new RelatorioFontResolver();
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPiuServices(builder.Configuration);

var app = builder.Build();

app.UseExceptionHandler();
app.UseCors("Web");
app.UseAuthentication();
app.UseAuthorization();

app.MapPiuEndpoints();

if (!app.Environment.IsEnvironment("Testing"))
{
    await app.SeedAsync();
}

app.Run();
