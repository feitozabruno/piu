using Piu.Api.Extensions;
using Piu.Api.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPiuServices(builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    await AdminSeeder.SeedAsync(scope.ServiceProvider, builder.Configuration);
}

app.UseExceptionHandler();
app.UseCors("Web");
app.UseAuthentication();
app.UseAuthorization();

app.MapPiuEndpoints();

app.Run();
