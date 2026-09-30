# ADR 002: Adoção de Vertical Slice Architecture e Domínio Rico

## Status
Aceito

## Contexto
No desenvolvimento do back-end (`Piu.Api`), precisamos definir como estruturar a base de código e onde as regras de negócio devem viver. A abordagem mais comum no ecossistema .NET (Clean Architecture) divide o projeto em camadas técnicas horizontais (Controllers, Services, Repositories). Isso gera a chamada "fadiga de camadas", exigindo a modificação de múltiplos arquivos e pastas distantes para implementar uma funcionalidade simples.

Além disso, o uso de Modelos Anêmicos (classes apenas com `get` e `set` públicos) resulta em regras de validação espalhadas por todo o sistema. O processo de um incubatório é altamente segmentado por etapas (Recebimento, Estocagem, Incubação, Nascimento) e possui regras físicas rígidas (ex: não existe quantidade negativa de ovos).

## Decisão
1. **Vertical Slice Architecture (VSA):** A estrutura de pastas será organizada por funcionalidades (Features) e não por camadas técnicas. Todo o código necessário para uma operação de negócio existir (Endpoint da API, DTOs/Commands, Lógica e Acesso a Dados) residirá dentro da mesma pasta da Feature (ex: `Features/Recebimento`). Preterimos a Clean Architecture para reduzir a complexidade e focar na entrega de valor funcional.
2. **Domínio Rico (DDD Essencial):** Adotaremos os princípios de Domain-Driven Design para o núcleo do sistema. As entidades de domínio deverão proteger sua própria integridade utilizando encapsulamento (propriedades com `private set`) e *Factory Methods* (métodos de criação estáticos). Uma entidade nunca deve ser instanciada em um estado inválido.

## Consequências
* **Positivas:** 
  * **Alta Coesão:** O código que muda junto, fica junto. Desenvolvedores não precisam pular entre projetos ou pastas diferentes para entender uma funcionalidade de ponta a ponta.
  * **Alinhamento com o Negócio:** A estrutura do projeto reflete exatamente os processos reais do chão de fábrica do incubatório.
  * **Segurança e Previsibilidade:** Entidades ricas impedem que dados corrompidos ou regras de negócio inválidas cheguem ao banco de dados, centralizando a validação.
* **Negativas / Desafios:**
  * **Duplicação Tolerada:** Compartilhar código entre Slices diferentes é desencorajado. Isso pode resultar em certa duplicação de código (ex: DTOs parecidos em Slices diferentes), o que exige uma mudança de mentalidade para aceitar que o acoplamento é pior que a duplicação.
  * **Curva de Aprendizado:** Exige disciplina para encapsular comportamentos dentro da Entidade em vez de criar "Services" genéricos.
