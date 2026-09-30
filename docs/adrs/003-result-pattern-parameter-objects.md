# ADR 003: Uso do Result Pattern e Parameter Objects

## Status
Aceito

## Contexto
Durante a modelagem das regras de negócio do Domínio, identificamos dois problemas arquiteturais comuns:
1. **Exception-Driven Development (Controle de fluxo por exceções):** Lançar exceções (`throw new ArgumentException`) para validações de regras de negócio (como tentar registrar uma quantidade negativa de ovos) é custoso para a performance da aplicação (devido ao *stack trace*) e dificulta a leitura do fluxo esperado da aplicação. Exceções devem ser exclusivas para cenários excepcionais e falhas de infraestrutura (ex: banco de dados indisponível).
2. **Ambiguidade de Argumentos:** Construtores ou métodos de fábrica que recebem múltiplos parâmetros do mesmo tipo (ex: `int ovosNinho, int ovosCama`) abrem brechas para falhas humanas, onde o desenvolvedor pode inverter a ordem dos parâmetros no momento da chamada, e o compilador não acusará erro.

## Decisão
1. **Result Pattern:** Adotaremos o padrão `Result<T>` para encapsular o retorno de operações de negócio. Qualquer método que possa falhar por quebra de regra de negócio retornará um objeto indicando o sucesso (`IsSuccess = true`) ou a falha contendo a mensagem de erro. As APIs deverão inspecionar esse resultado e traduzi-lo para o HTTP Status Code adequado (ex: `400 Bad Request`).
2. **Parameter Objects:** Substituiremos longas listas de argumentos posicionais por "Objetos de Parâmetros". Utilizaremos os `records` modernos do C# combinados com as palavras-chave `required` e `init`. Isso obriga o chamador do método a nomear explicitamente qual propriedade está sendo preenchida, impedindo omissões ou inversões de ordem.

## Consequências
* **Positivas:** 
  * **Segurança de Tipos e Semântica:** Impossível inverter `OvosNinho` com `OvosCama` acidentalmente, pois a nomeação é obrigatória.
  * **Performance e Previsibilidade:** O fluxo da aplicação não é interrompido bruscamente por exceções esperadas, tornando o consumo das classes pela Minimal API extremamente previsível e rápido.
  * **Contratos Claros:** O `Parameter Object` serve perfeitamente como o contrato (DTO/Command) que o front-end envia para a API, reduzindo a necessidade de mapeamentos complexos.
* **Negativas / Desafios:**
  * **Leve Verbosidade:** O desenvolvedor precisa instanciar o `record` de argumentos antes de chamar o método e deve sempre fazer o *unpack* (verificar `if (result.IsSuccess)`) do `Result Pattern` em vez de apenas confiar num bloco `try/catch` global.
