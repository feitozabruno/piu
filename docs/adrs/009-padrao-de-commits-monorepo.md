# ADR 009: Padrão de Versionamento (Conventional Commits para Monorepo)

## Status
Aceito

## Contexto
O Projeto "Piu" está estruturado como um *Monorepo*, ou seja, o mesmo repositório Git abriga tanto o código do Front-end (`Piu.Web`) quanto o do Back-end (`Piu.Api`). Se adotássemos o padrão *Conventional Commits* de forma simples (ex: `feat(auth): adiciona login`), seria impossível determinar, apenas lendo o histórico, se o desenvolvedor criou a tela de login no React ou o endpoint na API. Para manter um histórico de alterações rastreável, legível e profissional, precisamos de uma estratégia de escopo mais rigorosa.

## Decisão
Adotaremos o padrão **Conventional Commits com Escopo Composto**. A estrutura obrigatória da mensagem de commit será: `tipo(aplicacao/funcionalidade): descricao`.

**Regras do Escopo:**
1. **Aplicações Permitidas:** O primeiro termo do escopo deve obrigatoriamente indicar onde a mudança ocorreu:
   * `api`: Para alterações no C# (.NET).
   * `web`: Para alterações no Front-end (React/Vite).
   * `root`: Para arquivos estruturais globais na raiz do projeto (ex: `.sln`, `.editorconfig`, `.gitignore`, `docs/adrs`).
2. **Funcionalidades (Features):** O segundo termo indicará a Vertical Slice ou o contexto arquitetural modificado.
3. **Exemplos de Uso:**
   * `feat(api/recebimento): cria endpoint post para registrar lote inicial`
   * `feat(web/auth): cria tela de login para operacao offline`
   * `fix(api/lotes): corrige soma de saldo no livro razao`
   * `docs(root): adiciona adr 010 sobre convencao de commits`

## Consequências
* **Positivas:** 
  * **Rastreabilidade Imediata:** A leitura do `git log` revela exatamente qual camada do sistema foi alterada sem a necessidade de inspecionar os arquivos (diffs).
  * **Changelogs Automatizados:** No futuro, será extremamente fácil rodar scripts para gerar notas de atualização separadas para o aplicativo e para a API.
* **Negativas / Desafios:**
  * Exige disciplina rigorosa. Desenvolvedores não podem usar comandos genéricos como `git commit -m "ajustes"`. 
  * Requer atenção extra ao fazer commits que alteram tanto a API quanto o Web na mesma leva (nestes casos, o ideal é dividir a alteração em dois commits separados).
