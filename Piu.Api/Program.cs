using Piu.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPiuServices(builder.Configuration);

var app = builder.Build();

app.UseExceptionHandler();
app.UseCors("Web");
app.UseAuthentication();
app.UseAuthorization();

app.MapPiuEndpoints();

app.Run();
