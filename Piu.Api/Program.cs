using Piu.Api.Infrastructure.Persistence;
using Piu.Api.Middleware;
using Piu.Api.Features.Auth;
using Piu.Api.Features.Incubatorios;
using Piu.Api.Infrastructure;
using Piu.Api.Features.Lotes;
using Piu.Api.Features.Ovos;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGlobalExceptionHandling();
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddAuth();
builder.Services.AddHandlers();
builder.Services.AddValidation();
builder.Services.AddWebCors();

var app = builder.Build();

app.UseExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();
app.UseCors("Web");

app.MapAuth();
app.MapIncubatorios();
app.MapLotes();
app.MapOvos();
app.MapGet("/", () => "Hello, World!");

app.Run();
