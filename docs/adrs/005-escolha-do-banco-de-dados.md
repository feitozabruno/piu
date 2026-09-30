# ADR 005: Escolha do Banco de Dados (PostgreSQL)

## Status
Aceito

## Contexto
O sistema baseia-se em um modelo relacional e transacional (Entity Framework Core) para garantir o rigoroso controle do chão de fábrica (Identity de usuários, Lotes, e o Livro Razão de Movimentações). No ecossistema .NET, a escolha padrão costuma ser o Microsoft SQL Server. No entanto, licenças e hospedagem em nuvem (Azure SQL, AWS RDS) para SQL Server têm um custo inicial alto e opções gratuitas muito limitadas. Além da questão financeira do micro-SaaS, precisamos de um banco de dados que suporte bem dados híbridos, caso as Movimentações exijam campos flexíveis (como arrays de temperaturas ou fotos de descartes).

## Decisão
O banco de dados oficial do projeto será o **PostgreSQL**, orquestrado pelo Entity Framework Core utilizando o provedor oficial **Npgsql**.

## Consequências
* **Positivas:** 
  * **Custo Zero Inicial:** Adoção de arquiteturas *Serverless* e hospedagens com tiers gratuitos generosos e definitivos (como Neon.tech ou Supabase) sem sacrificar performance.
  * **Poder do JSONB:** O PostgreSQL possui suporte nativo e altamente otimizado ao tipo `JSONB`. O EF Core + Npgsql mapeiam isso nativamente, permitindo que as entidades tenham propriedades dinâmicas e flexíveis dentro de uma estrutura rigidamente relacional.
  * **Fuga do Vendor Lock-in:** Demonstra maturidade técnica ao dissociar a aplicação .NET da dependência exclusiva do ecossistema proprietário da Microsoft para dados.
* **Negativas / Desafios:**
  * Embora o Entity Framework Core abstraia quase 100% da diferença entre os bancos para o código C#, configurações muito específicas de infraestrutura, backup ou otimização de queries nativas exigirão conhecimento da sintaxe própria do Postgres.
