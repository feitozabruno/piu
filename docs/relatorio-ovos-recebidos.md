# Relatório de ovos recebidos

## Endpoint

`GET /api/v1/relatorios/ovos?dataInicial=2026-10-01&dataFinal=2026-10-03`

O endpoint exige autenticação. Um único dia com registros retorna `application/pdf`; dois ou
mais dias com registros retornam `application/zip`. Dias sem registros são ignorados. O
intervalo máximo é de 31 dias e um dia com mais de 10 registros retorna erro, pois o
formulário não possui linhas adicionais.

## Formulário e mapeamento

O arquivo [003290 - Controle de Ovos Recebidos das Granjas
Matrizes_V02.pdf](../003290%20-%20Controle%20de%20Ovos%20Recebidos%20das%20Granjas%20Matrizes_V02.pdf)
é copiado como recurso somente leitura para `Resources/` no diretório de execução.
O PDF tem uma página, tamanho 792 x 612 pontos, orientação paisagem. A origem das
coordenadas de desenho é o canto superior esquerdo e as unidades são pontos PDF.
A camada de texto usa a fonte livre `DejaVu Sans`, distribuída como recurso da aplicação e
resolvida diretamente pelo PdfSharp. Portanto, o relatório funciona tanto com
`dotnet run` quanto no container, sem depender de fontes instaladas no sistema.

O relatório usa a data de `HoraRecebimento`, filtra por intervalos UTC semiabertos e
restringe os dados ao incubatório do usuário autenticado. Como o projeto não possui um fuso
de negócio configurado, DATA e HORA são formatadas em UTC. O mapeamento acordado é:

| Campo do formulário | Origem |
| --- | --- |
| DATA | Data de `HoraRecebimento` |
| LOTE | `Lote.Nome` |
| AVIÁRIO | vazio (não existe origem no domínio atual) |
| POSTURA DIA ANTERIOR | `NinhoIncubavel` |
| POSTURA DO DIA | vazio (não existe origem no domínio atual) |
| Nº OVOS "X" | `CamaIncubavel` |
| Temperatura - Meio carrinho | `TemperaturaRecebimento` |
| PLACA | `PlacaCaminhao` |
| HORA | Hora de `HoraRecebimento` |
| TEMP: FURGÃO | `TemperaturaBau` |

As posições estão centralizadas em `RelatorioOvosService`. Uma nova versão do formulário
deve substituir o PDF vinculado no `Piu.Api.csproj`; depois, as coordenadas e o limite de
linhas devem ser revalidados visualmente.

## Validação

```bash
dotnet build Piu.Api/Piu.Api.csproj --no-restore
dotnet test Piu.Api.UnitTests/Piu.Api.UnitTests.csproj --no-restore
```

O teste unitário verifica a composição segura do ZIP. A validação visual do alinhamento
dos campos ainda deve ser feita com dados controlados em ambiente de homologação.
