# ADR 010: Validação de Borda (Edge Validation) e Fail-Fast

## Status
Aceito

## Contexto
Temos o Domínio blindado por *Value Objects* e *Result Pattern* (ADRs 003 e 007), que garantem que nenhuma regra de negócio seja violada. No entanto, requisições vindas do front-end podem conter dados malformados (nulos, strings vazias ou excedendo o limite do banco de dados). Se deixarmos essas validações básicas estruturais chegarem até o Domínio, estaremos misturando "regras de formatação web" com "regras de negócio agrícola", além de desperdiçar processamento instanciando objetos que fatalmente falharão.

## Decisão
Adotaremos o princípio de **Fail-Fast (Falhe Rápido)** implementando a Validação de Borda (Edge Validation):
1. **No Back-end (C#):** Utilizaremos a biblioteca **FluentValidation** acoplada aos *Parameter Objects / Commands* nas Minimal APIs. Se o JSON de entrada estiver malformado, a API retornará `400 Bad Request` imediatamente, impedindo que o código alcance a camada de Domínio.
2. **No Front-end (React):** O conceito será espelhado utilizando **React Hook Form + Zod** para validar os dados no momento da digitação do operador, evitando requisições desnecessárias (especialmente importante no cenário offline-first).
3. **Separação de Responsabilidades:** O FluentValidation validará apenas **estrutura e formato** (tamanho de string, campos obrigatórios, regex). Validações que exigem ida ao banco de dados ou regras intrínsecas do negócio continuarão no Domínio / Value Objects.

## Consequências
* **Positivas:** 
  * O Domínio permanece puro, focado apenas em regras de negócio.
  * Respostas rápidas e padronizadas para o cliente (Front-end) quando há erro de preenchimento.
  * O Front-end e o Back-end compartilham a mesma filosofia de validação (FluentValidation e Zod possuem sintaxes declarativas muito similares).
* **Negativas / Desafios:**
  * **Duplicação de Regras Básicas:** A regra de obrigatoriedade de um campo terá que ser escrita três vezes: no Zod (Front-end), no FluentValidation (API) e no EF Core (Banco de Dados). Aceitamos isso em prol da segurança em camadas.
