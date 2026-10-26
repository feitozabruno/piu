FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Piu.Api/Piu.Api.csproj Piu.Api/
COPY ["003290 - Controle de Ovos Recebidos das Granjas Matrizes_V02.pdf", "./"]
RUN dotnet restore Piu.Api/Piu.Api.csproj

COPY Piu.Api/ Piu.Api/
RUN dotnet publish Piu.Api/Piu.Api.csproj \
    --configuration Release \
    --no-restore \
    --output /app/publish \
    --property UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

RUN apt-get update \
    && apt-get install --no-install-recommends --yes fonts-dejavu \
    && rm -rf /var/lib/apt/lists/*

ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 10000

COPY --from=build /app/publish .

ENTRYPOINT ["sh", "-c", "dotnet Piu.Api.dll --urls http://0.0.0.0:${PORT:-10000}"]
