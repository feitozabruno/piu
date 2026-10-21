using Piu.Api.Features.Auth;
using Piu.Api.Features.Incubatorios;
using Piu.Api.Features.Lotes;
using Piu.Api.Features.Ovos;
using Piu.Api.Infrastructure;
using Piu.Api.Infrastructure.Persistence;
using Piu.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGlobalExceptionHandling();
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddAuth();
builder.Services.AddHandlers();
builder.Services.AddValidation();
builder.Services.AddWebCors();

var app = builder.Build();

app.UseExceptionHandler();

app.UseCors("Web");
app.UseAuthentication();
app.UseAuthorization();

app.MapAuth();
app.MapIncubatorios();
app.MapLotes();
app.MapOvos();
app.MapGet("/", () => "Hello, World!");

app.Run();
