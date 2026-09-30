# ADR 006: Autenticação, Autorização e Contexto de Usuário (Identity) para Ambientes Offline

## Status
Aceito

## Contexto
O projeto "Piu" é um PWA voltado para uso em chão de fábrica (incubatório), operando frequentemente sem conexão com a internet. A rastreabilidade das operações — saber *quem* executou uma ação — é tão crítica quanto o dado em si. Se implementarmos o sistema sem autenticação e decidirmos adicioná-la mais tarde, o custo de refatoração do banco de dados (Livro Razão) e das regras de domínio seria altíssimo. O desafio principal é: como autenticar e rastrear operadores que realizam ações enquanto o dispositivo está offline, sem deslogá-los ou perder os dados gerados?

## Decisão
1. **Obrigatoriedade de Autoria:** O contexto de usuário (`OperadorId`) será um requisito em todas as entidades cruciais e linhas do Livro Razão desde o Dia 1.
2. **Tecnologia:** Utilizaremos o **ASP.NET Core Identity API Endpoints** (nativo do .NET 8+), que fornece rotas padronizadas para geração e validação de tokens JWT (`Access Token` e `Refresh Token`), integrando nativamente com o Entity Framework e com a nossa classe customizada `Operador`.
3. **Estratégia Offline-First:** 
   * O Front-end (React/Vite) será responsável por manter os tokens (`Access Token` e `Refresh Token`) e os dados básicos do operador logado em armazenamento local.
   * Enquanto offline, o Front-end assinará as requisições pendentes usando o `OperadorId` salvo localmente.
   * No retorno da internet, se o `Access Token` estiver expirado (retornando `401 Unauthorized`), o Front-end interceptará o erro silenciosamente, enviará o `Refresh Token` para a API, renovará a sessão e retomará a fila de sincronização sem interromper o usuário.

## Consequências
* **Positivas:** 
  * O Livro Razão fica irrepreensível para fins de auditoria (sempre saberemos quem apertou o botão, com ou sem internet).
  * Evita a dor de cabeça e os problemas de segurança de configurar pacotes JWT de terceiros no C#.
  * A experiência do usuário não é afetada por *logouts* forçados devido a instabilidades na rede da fábrica.
* **Negativas / Desafios:**
  * **Complexidade no Front-end:** Exige a configuração de um interceptor inteligente (no Axios ou Fetch) no React para lidar com a expiração do token e renovação silenciosa antes de drenar a fila de dados offline.
