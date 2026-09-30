# ADR 008: Linguagem Ubíqua e Idioma do Código

## Status
Aceito

## Contexto
No desenvolvimento de software corporativo no Brasil, existe o debate constante sobre escrever o código totalmente em inglês ou em português. Como adotamos os princípios de Domain-Driven Design (DDD), a regra fundamental é o uso da **Linguagem Ubíqua (Ubiquitous Language)** — o código deve refletir exatamente os termos utilizados pelos especialistas do negócio no chão de fábrica do incubatório. A tentativa de traduzir termos altamente específicos (como "Ovos Ninho", "Ovos Cama" ou "Data de Postura") para o inglês cria uma barreira de tradução mental (carga cognitiva) para os desenvolvedores e gera constantes ruídos e bugs na comunicação com a operação.

## Decisão
Adotaremos o **Padrão Híbrido** de nomenclatura em toda a solução:
1. **Domínio em Português (sem acentos ou caracteres especiais):** Entidades, Propriedades, Value Objects e Regras de Negócio utilizarão a língua nativa do incubatório (ex: `Lote`, `OvosNinho`, `TemperaturaBau`, `RegistrarDescarte()`).
2. **Infraestrutura e Padrões em Inglês:** Nomes de arquivos de configuração, sufixos arquiteturais e infraestrutura utilizarão os termos universais da engenharia de software (ex: `LoteRepository`, `RecebimentoController`, `CriarLoteCommand`, `Result<T>`).

## Consequências
* **Positivas:** 
  * **Comunicação Cristalina:** Quando o encarregado relatar um problema nos "Ovos Cama do Lote", o desenvolvedor buscará exatamente a propriedade `OvosCama` dentro da entidade `Lote`. Fim da adivinhação.
  * **Manutenção da Fluência Técnica:** O uso de padrões globais (como `Command` ou `Controller`) mantém o código padronizado com o restante do ecossistema .NET.
* **Negativas / Desafios:**
  * O código apresentará uma mescla de idiomas (frequentemente chamada de "Portinglês"), o que pode causar certa estranheza visual nas primeiras semanas de desenvolvimento.
  * Para fins de portfólio internacional, será obrigatório incluir uma nota explicativa no `README.md` justificando a adoção do português no Domínio como uma decisão arquitetural baseada em DDD.
