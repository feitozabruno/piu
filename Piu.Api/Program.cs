using Piu.Api.Data;
using Piu.Api.Extensions;
using Piu.Api.Features.Auth;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGlobalExceptionHandling();
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddAuth();
builder.Services.AddValidation();

var app = builder.Build();

app.UseExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();

app.MapAuth();
app.MapGet("/", () => "Hello, World!");

app.Run();
