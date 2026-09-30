# ADR 007: Uso Estratégico de Value Objects (VO)

## Status
Aceito

## Contexto
Em sistemas corporativos, é comum representar grandezas do mundo real com tipos primitivos básicos da linguagem (`int`, `decimal`, `string`). Isso gera um *anti-pattern* conhecido como "Obsessão por Primitivos" (*Primitive Obsession*). 
No domínio do incubatório, valores como `Temperatura` ou `QuantidadeDeOvos` possuem limites físicos rígidos (ex: temperatura não pode ser 500ºC, ovos não podem ser negativos). Usar tipos primitivos espalha a lógica de validação (`if temp < 0`) por toda a aplicação (APIs, Serviços, Front-end). Além disso, um tipo `decimal` não tem significado semântico, permitindo que um método receba o peso do caminhão no lugar da temperatura sem que o compilador acuse erro.

## Decisão
1. **Encapsulamento de Regras:** Conceitos vitais do domínio que possuem validações intrínsecas serão modelados como **Value Objects (VO)**, utilizando `records` do C# com construtores privados e métodos estáticos de fábrica (retornando o *Result Pattern*).
2. **Pragmatismo (Evitando Over-engineering):** VOs serão aplicados **apenas** para grandezas críticas que podem corromper o domínio se receberem dados inválidos (como Temperaturas, Quantidades e Identificação da Granja). Propriedades meramente descritivas (como Placa do Caminhão ou Justificativa de Descarte) permanecerão como tipos primitivos (`string`).
3. **Mapeamento de Banco de Dados:** Para evitar a complexidade histórica de persistir VOs no banco de dados, utilizaremos o recurso de **Complex Types** introduzido no Entity Framework Core 8, que "achata" automaticamente as propriedades do VO em colunas primitivas na tabela do Postgres, de forma transparente.

## Consequências
* **Positivas:** 
  * **Validação Centralizada:** A regra do que constitui uma "Temperatura Válida" existe em apenas um único lugar no sistema. Se a regra mudar, altera-se apenas o VO.
  * **Segurança de Tipos (Type Safety):** Impossível passar a grandeza errada para um método, pois ele exigirá explicitamente o tipo `Temperatura` em vez de um `decimal` genérico.
* **Negativas / Desafios:**
  * **Curva de Aprendizado:** Desenvolvedores menos experientes com DDD podem estranhar a necessidade de criar uma classe inteira para encapsular um simples número.
