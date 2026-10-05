using Piu.Api.Data;
using Piu.Api.Features.Auth;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddAuth();
builder.Services.AddValidation();

builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = contexto =>
    {
        if (contexto.ProblemDetails is HttpValidationProblemDetails)
        {
            contexto.ProblemDetails.Title = "Um ou mais erros de validação ocorreram.";
        }
    };
});

var app = builder.Build();
app.UseExceptionHandler();
app.UseStatusCodePages();
app.UseAuthentication();
app.UseAuthorization();
app.MapAuth();

app.MapGet("/", () => "Hello, World!");

app.Run();
