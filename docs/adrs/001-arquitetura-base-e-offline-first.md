# ADR 001: Arquitetura Base e Estratégia de Offline-First

## Status
Aceito

## Contexto
O processo de recebimento incubação de ovos ocorre no chão de fábrica, onde a conexão com a internet é instável ou inexistente. Precisamos de um sistema para substituir as planilhas de papel, garantindo que os operadores não percam dados durante quedas de rede.

## Decisão
1. O Front-end será desenvolvido em **React (PWA)**, permitindo instalação nos dispositivos móveis dos operadores e armazenamento local via `IndexedDB/localStorage`.
2. O Back-end será desenvolvido em **C# / ASP.NET Core (Web API)**, atuando como o servidor central que receberá os dados quando houver conexão.
3. Os IDs dos registros serão gerados no Front-end utilizando **GUIDs (UUIDs)** para evitar conflitos de ID quando o celular sincronizar os dados offline com o servidor.

## Consequências
* **Positivas:** Operação ininterrupta no chão de fábrica; alta escalabilidade e segurança com o ecossistema .NET.
* **Negativas/Desafios:** Adiciona complexidade na sincronização de dados (lidar com falhas de rede na hora de enviar o pacote).
