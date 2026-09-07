# Proposta de AGENTS.md — FinanceNow

Este arquivo é uma proposta para revisão. Para adotá-lo, coloque seu conteúdo em `AGENTS.md` na raiz. Os caminhos abaixo são relativos à raiz do repositório.

## Escopo e arquitetura

- A solução principal é `FinanceNowProject.sln`: API, Data, Modelos e Web.
- `FinanceNowProject/FinanceNow.API.csproj`: ASP.NET Core, controllers, DTOs, AutoMapper, serviços e configuração de autenticação.
- `FinanceNow.Data`: EF Core/SQL Server, Context, repositórios e migrations da API.
- `FinanceNow.Modelos`: entidades e enum TipoDeTransacao. Atualmente também depende de Identity.
- `FinanceNow.Web`: Blazor Interactive Server, cliente HTTP e Identity próprio.
- `FinanceNow.FrontEnd` e `FinanceNow.Blazor` estão fora da solução. Só altere essas alternativas quando a tarefa as envolver; não as remova por presumir que são descartáveis.
- Preserve o fluxo de transações controller → service → repository → Context. Regras de negócio devem ficar no serviço; o controller trata HTTP e o repositório trata persistência. Categoria ainda acessa Context diretamente: evite refatorações amplas sem relação com a tarefa.

## Antes de alterar

1. Leia `git status --short` e o diff dos arquivos envolvidos. Preserve alterações anteriores do usuário, inclusive arquivos ainda não rastreados.
2. Leia os DTOs, a rota, o cliente Web e o mapeamento EF envolvidos. Alterações em um contrato exigem conferir os dois lados.
3. Identifique um comportamento observável e seu critério de aceitação. Para um defeito funcional, reproduza-o e adicione um teste de regressão quando houver infraestrutura apropriada.
4. Não use `git reset --hard`, `git clean`, aplicação de migrations ou remoção de bancos como forma de resolver problemas de ambiente. Use banco descartável explicitamente identificado para testes de escrita.

## Comandos de desenvolvimento

```powershell
dotnet --info
dotnet restore FinanceNowProject.sln
dotnet build FinanceNowProject.sln --no-restore
dotnet test FinanceNowProject.sln --no-build --no-restore --verbosity normal
dotnet list FinanceNowProject.sln package --include-transitive --vulnerable --no-restore
```

Em setembro de 2026, API/Data/Modelos usam net10.0 e Web usa net9.0. Não há global.json. Confira os csproj antes de mudar SDK ou pacotes. Não suprima avisos de segurança para obter build verde. Não declare testes aprovados se nenhum teste foi descoberto: hoje não há projeto de testes.

Execute em terminais separados, com certificado HTTPS de desenvolvimento válido:

```powershell
dotnet run --project FinanceNowProject/FinanceNow.API.csproj --launch-profile https
dotnet run --project FinanceNow.Web/FinanceNow.Web.csproj --launch-profile https
```

API: https://localhost:7143; Swagger: /swagger. Web: https://localhost:7042. A Web atualmente fixa a URL da API no Program.cs. O perfil HTTP isolado da API não atende essa URL HTTPS.

## Banco e configuração

- A API lê `ConnectionStrings:Connection`; o perfil Development usa LocalDB/FinanceNowDataBase.
- A Web lê `ConnectionStrings:DefaultConnection` e usa outro banco Identity.
- Nunca registre senhas, tokens, cookies ou registros financeiros nos relatórios de teste. Use configuração local/variáveis de ambiente para valores sensíveis.
- Confira `dotnet ef --version`. Prefira ferramenta local alinhada à versão EF do projeto quando essa configuração for adicionada.
- Para consultar migrations: `dotnet ef migrations list --project FinanceNow.Data --startup-project FinanceNowProject --context Context --no-build`; na Web use `--project FinanceNow.Web --startup-project FinanceNow.Web --context ApplicationDbContext`.
- Para conferir modelo/snapshot, substitua `migrations list` por `migrations has-pending-model-changes`. Isso não comprova que o banco está atualizado.
- Crie novas migrations para mudanças de esquema; revise o SQL e a preservação de dados. Não reescreva migrations já aplicadas. Não aplique mudanças no banco pessoal para executar testes automatizados.

## Regras de implementação e autorização

- Use async/await de ponta a ponta e propague CancellationToken. Não inicie operações EF sem await nem execute consultas concorrentes no mesmo Context.
- Dados inválidos devem resultar em 400; recurso inexistente em 404; falhas inesperadas em 500 com resposta segura. Use ProblemDetails consistentemente.
- Valide enum com valores definidos, período, categoria existente e compatibilidade do tipo categoria/transação. Required em int/enum/DateOnly não rejeita automaticamente todos os valores default.
- Preserve o contrato JSON consumido pela Web, inclusive o uso atual de `$values`, até uma alteração coordenada de API e cliente.
- Para novos valores monetários prefira decimal e precisão SQL explícita. A migração do double/float existente precisa de teste e revisão de arredondamento.
- Para recorrência, teste quantidade, fim de mês, virada do ano e atomicidade. Todas as parcelas devem persistir juntas ou nenhuma.
- Nunca retire autorização para fazer um teste passar. A API é o local de imposição das permissões; ocultar uma página Blazor não protege endpoints.
- Ao implementar isolamento, obtenha o dono pela identidade autenticada e filtre leitura, listagem, atualização e exclusão. Nunca confie em UserId enviado pelo cliente. Teste usuário A acessando IDs de B.
- Defina explicitamente se categorias são globais ou por usuário; categorias globais precisam de política de escrita adequada.
- MapIdentityApi usa tokens próprios do Identity, não JWT. Login Web e login API atualmente não compartilham autenticação. Verifique o mecanismo real antes de configurar Swagger, logout ou propagação de credenciais.

## Testes e entrega

- Prioridade: acesso anônimo negado, isolamento A/B, CRUD de receitas/despesas, filtro mensal, validação, 404 e recorrência atômica.
- Testes unitários verificam regras. Integração HTTP deve exercitar autorização real e persistência relacional em banco isolado. Não use EF InMemory como prova de constraints/transações SQL Server.
- Testes de autorização podem usar identidade simulada para matriz de permissões, mas devem ser complementados por casos do login/refresh/cookie ou bearer real adotado.
- Após mudanças, rode build e testes pertinentes. Mudança de rotas exige verificar chamadas da Web; mudança de schema exige conferir migration e execução em banco descartável.
- Não edite bin, obj, .vs ou arquivos *.user. Como há artefatos já versionados, selecione os arquivos do commit explicitamente; não use git add . indiscriminadamente.
- Na entrega, explique comportamento alterado, comandos executados, resultados, testes realmente descobertos e limitações. Distinga observação em execução de hipótese baseada no código.
