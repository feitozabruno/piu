# ADR 004: Modelagem de Lotes baseada em Livro Razão (Append-Only Ledger)

## Status
Aceito

## Contexto
Na indústria de alimentos e incubatórios, a rastreabilidade dos insumos (ovos férteis) é uma exigência legal e operacional crítica. Tradicionalmente, sistemas atualizam o saldo de estoque alterando diretamente uma coluna no banco de dados (ex: `UPDATE Lotes SET Quantidade = Quantidade - 50`). Essa abordagem destrói o histórico: perde-se a informação de *quem*, *quando* e *por que* aqueles ovos foram retirados. 

Além disso, como o sistema operará em modo *offline-first*, múltiplos operadores poderiam tentar atualizar o saldo do mesmo lote simultaneamente sem internet. Sincronizar atualizações de estado direto (`UPDATES`) geraria conflitos complexos de resolver na API.

## Decisão
O Agregado Principal (Aggregate Root) `Lote` será modelado utilizando o padrão de **Livro Razão (Append-Only Ledger)**. 
1. **Eventos/Movimentações:** Todas as ações (Recebimento inicial, Descarte, Transferência e Nascimento) gerarão uma nova linha imutável de `Movimentacao` atrelada ao Lote.
2. **Imutabilidade:** Uma `Movimentacao` registrada nunca poderá ser alterada ou apagada. Correções exigirão uma movimentação compensatória.
3. **Cálculo de Saldo Dinâmico:** As propriedades de saldo atual do Lote (ex: `SaldoOvosNinho`, `TotalOvosAtual`) não serão colunas físicas estáticas na tabela do Lote, mas sim propriedades computadas dinamicamente pela soma de todas as movimentações do seu Livro Razão.

## Consequências
* **Positivas:** 
  * **Rastreabilidade e Auditoria Absoluta:** O sistema saberá exatamente a linha do tempo de cada perda ou ganho de um lote.
  * **Sincronização Offline Sem Conflitos:** Quando a internet voltar, o celular apenas envia as "novas movimentações" para a API (que fará um simples `INSERT` na ponta). Não há conflito de saldo, a API apenas calcula o novo total após adicionar a linha.
  * **Inteligência de Dados (Analytics):** No futuro, será possível extrair gráficos preditivos vitais, como a curva exata do dia de maior mortalidade por granja de origem.
* **Negativas / Desafios:**
  * **Leitura mais custosa:** O cálculo do saldo exige somar as movimentações (o que no banco de dados se traduz num `SUM()` com `GROUP BY`). 
  * **Mitigação do Desafio:** Como o ciclo de vida de um lote num incubatório é curto (geralmente ~21 dias) e gera um número pequeno de movimentações por lote, o impacto de performance de calcular esse saldo dinamicamente (mesmo para milhares de lotes) é ínfimo em bancos de dados modernos como o PostgreSQL.
