# Análise do FinanceNow — 06/09/2026

A solução principal compila, mas o fluxo financeiro pela Web falha e a API financeira não exige autenticação. Compilar não prova que rotas, permissões e regras de negócio funcionam. A prioridade é estabelecer testes observáveis para esses comportamentos.

Análise feita sobre os arquivos atuais, incluindo alterações locais anteriores e projetos não rastreados. Não foram corrigidas regras de negócio nem aplicadas migrations. Foram criados apenas este relatório e a proposta de AGENTS; restore/build também atualizam artefatos gerados, muitos dos quais já estão versionados.

## Arquitetura, projetos e versões

É uma aplicação em camadas com API HTTP e interfaces Blazor, não uma arquitetura de microsserviços. Fluxo principal: Web → HTTP → TransacaoController → ITransacaoService/TransacaoService → ITransacaoRepository/TransacaoRepository → Context → SQL Server. AutoMapper transforma DTOs em entidades e respostas. CategoriaController acessa Context diretamente: a separação em camadas ainda é parcial.

| Projeto | Na solução | Framework | Responsabilidade e dependências diretas |
|---|---|---|---|
| FinanceNow.API, pasta FinanceNowProject | Sim | net10.0 | Referencia Data; AutoMapper 15.1.0; Authentication.JwtBearer, Identity.EntityFrameworkCore, OpenApi, EF Design/SqlServer/Tools 9.0.10; Microsoft.Build 17.14.28; Web.CodeGeneration.Design 9.0.0; SwaggerGen/SwaggerUI 9.0.6 |
| FinanceNow.Data | Sim | net10.0 | Referencia Modelos; Identity 2.3.1; Identity.EntityFrameworkCore e EF SqlServer/Tools 9.0.10; persistência e 7 migrations |
| FinanceNow.Modelos | Sim | net10.0 | Identity.EntityFrameworkCore 9.0.10; Transacao, Categoria, ApplicationUser e TipoDeTransacao |
| FinanceNow.Web | Sim | net9.0 | Blazor Interactive Server; Identity próprio; Diagnostics.EntityFrameworkCore, Identity.EntityFrameworkCore, EF SqlServer/Tools 9.0.10; 1 migration Identity |
| FinanceNow.FrontEnd | Não | net9.0 | Host Blazor WebAssembly; referencia seu Client; WebAssembly.Server, Diagnostics.EntityFrameworkCore, Identity.EntityFrameworkCore, EF SqlServer/Tools 9.0.2; Identity e migration próprios |
| FinanceNow.FrontEnd.Client | Não | net9.0 | WebAssembly e WebAssembly.Authentication 9.0.2 |
| FinanceNow.Blazor | Não | net9.0 | Host WebAssembly; WebAssembly.Server 9.0.1; falta ProjectReference para seu Client |
| FinanceNow.Blazor.Client | Não | net9.0 | WebAssembly 9.0.1 |

Todos habilitam nullable e implicit usings; não há LangVersion explícita. SDKs instalados: 9.0.313 e 10.0.400; selecionado 10.0.400 por ausência de global.json. ASP.NET Core 9.0.19 e 10.0.11 estão disponíveis. dotnet-ef global 10.0.8; dotnet-stryker global 4.0.6; nenhuma ferramenta local configurada. Ter Stryker instalado não significa que existam testes de mutação executáveis.

O backend usar net10.0 com EF 9 não impediu esta compilação, mas as versões divergentes e Identity 2.3.1 dificultam atualização e auditoria. Modelos está acoplado ao Identity/EF; para este tamanho de projeto isso não exige uma reestruturação imediata.

O .NET 9 está em manutenção com fim de suporte em 10/11/2026. Planejar uniformização em .NET 10 LTS é adequado, sem atualizar todos os pacotes cegamente. [Política oficial](https://dotnet.microsoft.com/en-us/platform/support/policy).

## Resultados reais das verificações

| Verificação | Resultado |
|---|---|
| dotnet restore FinanceNowProject.sln | Sucesso fora do sandbox; avisos NU1903 e NU1901 |
| dotnet build FinanceNowProject.sln --no-restore --verbosity minimal | Sucesso, 60 avisos, 0 erros, cerca de 5 s; build Debug com os artefatos locais existentes |
| dotnet test FinanceNowProject.sln --no-build --no-restore --verbosity normal | Exit code 0, mas nenhum teste executado e nenhum assembly de testes identificado |
| Auditoria NuGet com --include-transitive --vulnerable --no-restore | AutoMapper 15.1.0: alta; System.Security.Cryptography.Xml 8.0.2 transitivo em Data: alta; NuGet.Packaging e NuGet.Protocol 6.11.0: baixa. Nenhum pacote vulnerável reportado em Modelos/Web pelas fontes consultadas |
| API e Web com dotnet run, perfil https | Ambas iniciaram em Development; API 7143/5185, Web 7042/5151 |
| EF has-pending-model-changes, Context e ApplicationDbContext Web | Ambos sem diferenças em relação aos respectivos snapshots |
| EF migrations list, consultando os bancos locais | API: 7 migrations sem pendências indicadas. Web: CreateIdentitySchema pendente |
| Restore e build de FinanceNow.FrontEnd, fora da solução | Restore passou; build passou com 4 avisos de nulabilidade. Antes do restore havia NETSDK1112 por runtime browser-wasm ausente |
| Restore e build de FinanceNow.Blazor, fora da solução | Restore passou; build falhou com 12 CS0234: namespace Client sem referência de projeto |

Os avisos do build principal incluem nulabilidade (CS8602/CS8618), documentação XML (CS1591/CS1572/CS1573) e parâmetro não utilizado (CS9113), além dos avisos NuGet. A contagem pode cair em build incremental; não a use como métrica fixa. Não foi feito build Release nem validação em clone limpo.

Os GETs abaixo foram feitos sem cookie nem Authorization, usando HTTPS normal, sem ignorar a validação do certificado:

| Requisição | HTTP observado | Interpretação |
|---|---|---|
| API /swagger/v1/swagger.json | 200 | Documento Swagger disponível |
| API /Api/Categoria/Tipo/invalido | 400 | Rejeição do texto inválido funciona |
| API /Api/Categoria/Tipo/Receita | 200 | Consulta financeira acessível anonimamente; acesso ao banco funcionou |
| API /Api/Transacao/2147483647 | 500 | Log confirmou KeyNotFoundException, deveria ser 404 |
| API /Api/Transacao?ano=2026&mes=0&tipo=Receita | 500 | Log confirmou ArgumentOutOfRangeException, deveria ser 400 |
| API /Api/Transacao?ano=2026&mes=9&tipo=invalido | 200 | Tipo inválido aceito, deveria ser 400 |
| API /Api/Transacoes/2026/9/Receita | 404 | Rota antiga usada pela interface |
| Web / | 200 | Página inicial disponível |
| Web /Financeiro/Receita | 500 | Log confirmou HttpRequestException causada pelo 404 da API |
| API /Auth/manage/info | 401 | Proteção desse endpoint Identity funciona |

Limites: não foram executados cadastro/login com credenciais, refresh/logout autenticados, POST/PUT/DELETE financeiros, teste entre dois usuários, recorrência ou navegação interativa em navegador. Não houve escrita deliberada em dados financeiros. O sucesso de uma consulta não comprova todos os fluxos de banco. As aplicações iniciadas para análise foram encerradas ao final.

Restrições de ambiente foram separadas dos defeitos: NuGet.Config e DataProtection-Keys deram acesso negado no sandbox, e curl/Schannel não obteve credenciais. Reexecuções autorizadas fora do sandbox resolveram essas limitações. `dotnet dev-certs https --check` no sandbox disse não haver certificado válido, mas as chamadas HTTPS fora dele funcionaram; não é evidência de certificado ausente no usuário. Git apresentou dubious ownership do usuário isolado; a inspeção usou safe.directory somente no comando, sem alterar configuração global.

## Problemas priorizados e evidência no código

1. **Crítico: autorização financeira ausente.** CategoriaController tem Authorize comentado; TransacaoController não tem Authorize; não há fallback policy em Program.cs. Transacao e Categoria não possuem dono, e consultas não filtram por usuário. A leitura anônima foi confirmada; a falta de proteção em escrita foi constatada no código, sem modificar dados. Apenas adicionar Authorize ainda permitiria acesso cruzado entre usuários autenticados se os dados forem pessoais.
2. **Alto: contrato Web/API divergente.** Receita.razor e Despesa.razor chamam `Transacoes/{ano}/{mes}/{tipo}`; o contrato atual é `GET /Api/Transacao?ano=...&mes=...&tipo=...`. POST/PUT também usam plural; Despesa ainda usa PUT com ID no caminho, enquanto API usa PUT sem ID na rota. A página de receitas falhou em execução. A URL da API é fixa no Program da Web.
3. **Alto: respostas e fluxo assíncrono incorretos.** GetByPeriodo chama Problem sem retornar; Enum.TryParse também aceita números não definidos sem Enum.IsDefined. O serviço lança exceções para período inválido sem tradução para HTTP. FindTransacaoByAsync lança em ausência, tornando ineficazes os testes de null que deveriam gerar 404. UpdateAsync do serviço esquece await: `exists` é Task, não a transação, e pode iniciar operações concorrentes no mesmo Context. Este último foi identificado por leitura, não por PUT real.
4. **Alto: dependências vulneráveis.** A auditoria apontou versões diretas e transitivas afetadas. Investigue a cadeia e atualize/remova a origem; não basta ocultar NU1903. Microsoft.Build e ferramentas de scaffolding devem ter sua necessidade na API avaliada. AutoMapper também emitiu aviso de licença em execução: desenvolvimento/testes permitidos pelo aviso, produção exige revisão da licença.
5. **Alto: identidade fragmentada e banco Web pendente.** API usa IdentityUser<int> no banco financeiro; Web usa IdentityUser com chave string em outro banco. HttpClient não transmite identidade para a API. A Web exige conta confirmada e usa IdentityNoOpEmailSender: o envio de confirmação não está implementado. Sua migration inicial está pendente. Essas condições impedem considerar o fluxo completo de autenticação validado.
6. **Médio: integridade financeira.** Valor é double/float; decimal com precisão explícita é mais adequado para centavos. Recorrência salva a primeira transação e depois outras 11 em outro SaveChanges: se a segunda etapa falhar, a primeira permanece. Regras estão no repositório. Update DTO não tem Recorrente, e mapear para entidade nova pode redefini-lo para false. A relação categoria/transações está configurada no snapshot com delete cascade: excluir categoria pode excluir histórico associado; definir comportamento desejado antes de corrigir.
7. **Médio: validação e contrato inconsistentes.** Required em tipos valor não impede enum 0/data default/categoria 0. Não há validação explícita da compatibilidade entre tipo da categoria e transação. Categoria DTO aceita 15 caracteres e entidade 50; Web aceita descrição 30 e API 50; Web exige valor maior que zero e API aceita zero. ReferenceHandler.Preserve produz metadados `$id`/`$values`, dos quais ApiResponse da Web depende. GetById de categoria retorna entidade, enquanto consulta por tipo usa DTO. Padronizar sem quebrar silenciosamente o consumidor.
8. **Médio: baixa reprodutibilidade.** README tem apenas o título; não há AGENTS, global.json, manifesto local de ferramentas, testes, pipeline CI encontrado, .gitignore ou configuração de contêiner encontrada. Há bin/obj/.vs versionados e muitas mudanças anteriores; foram contados 284 arquivos rastreados entre dll/pdb/vsidx. O arquivo .http ainda chama weatherforecast, que não é endpoint atual. Há três alternativas de UI e só Web integra a solução.

O Swagger anuncia JWT, mas `AddIdentityApiEndpoints`/`MapIdentityApi` emitem tokens próprios do Identity, não JWT; ter referência ao pacote JwtBearer não configura AddJwtBearer. Logout por SignOutAsync também não deve ser apresentado como revogação imediata de qualquer bearer já emitido. [Documentação oficial do Identity](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity-api-authorization?view=aspnetcore-10.0).

## Como restaurar, compilar, executar e testar

Na raiz, com SDK 10 e runtime ASP.NET Core 9 disponíveis:

```powershell
dotnet restore FinanceNowProject.sln
dotnet build FinanceNowProject.sln --no-restore
dotnet test FinanceNowProject.sln --no-build --no-restore --verbosity normal
dotnet list FinanceNowProject.sln package --include-transitive --vulnerable --no-restore
```

Use SQL Server LocalDB no Windows. API lê `ConnectionStrings:Connection` em Development e usa FinanceNowDataBase; Web lê `ConnectionStrings:DefaultConnection` e usa o banco `aspnet-FinanceNow.Web-e7ada33b-1f46-4970-a640-5ee79e73e8e5`. Para outro ambiente configure essas chaves por variáveis de ambiente (`ConnectionStrings__Connection` e `ConnectionStrings__DefaultConnection`). A API não traz conexão em appsettings.json geral.

Antes de alterar banco, consulte o estado:

```powershell
dotnet ef migrations list --project FinanceNow.Data --startup-project FinanceNowProject --context Context
dotnet ef migrations list --project FinanceNow.Web --startup-project FinanceNow.Web --context ApplicationDbContext
```

Para provisionar **um banco de desenvolvimento novo e explicitamente selecionado**, os comandos são os seguintes; não foram executados nesta análise:

```powershell
dotnet ef database update --project FinanceNow.Data --startup-project FinanceNowProject --context Context
dotnet ef database update --project FinanceNow.Web --startup-project FinanceNow.Web --context ApplicationDbContext
```

É necessário conferir a conexão efetiva e o ambiente antes de aplicar. Nos perfis launchSettings o ambiente é Development; para comandos EF que não adotem o perfil, configure ASPNETCORE_ENVIRONMENT=Development ou passe a conexão de desenvolvimento explicitamente. O ef global 10.0.8 funcionou nesta máquina, mas o projeto deve futuramente fixar ferramenta local compatível com seu EF.

Verifique o certificado com `dotnet dev-certs https --check` no usuário desenvolvedor. Se faltar, `dotnet dev-certs https --trust` provisiona/confia no certificado de desenvolvimento. Inicie em dois terminais:

```powershell
dotnet run --project FinanceNowProject/FinanceNow.API.csproj --launch-profile https
dotnet run --project FinanceNow.Web/FinanceNow.Web.csproj --launch-profile https
```

Acesse https://localhost:7143/swagger e https://localhost:7042. O perfil http da API só publica 5185 e é insuficiente para o HttpClient atual da Web. Para projetos alternativos, restore/build precisam apontar ao csproj fora da solução; o host FinanceNow.Blazor continuará falhando enquanto faltar a referência ao Client.

## Lacunas de testes e critérios de aceitação

Não foi localizado xUnit, NUnit, MSTest, Microsoft.NET.Test.Sdk, projeto de testes ou suíte automatizada equivalente. O comando test atualmente oferece uma falsa sensação de validação se seu exit code for considerado isoladamente.

| Prioridade | Casos a adicionar | O que comprovam |
|---|---|---|
| P0 | Anônimo e token inválido em GET/POST/PUT/DELETE; A tentando ler/listar/editar/excluir dados de B; ID de dono forjado no corpo | 401 em não autenticado e 403/404 conforme contrato de recurso alheio; dados de B intactos |
| P0 | Login válido/inválido, confirmação, refresh, cookie/bearer real e logout conforme mecanismo escolhido | Identidade realmente chega à API e a semântica de sessão está documentada |
| P1 | CRUD de receita e despesa; URL Location após criação; filtro por mês/ano/tipo; persistência e exclusão | Caminho principal completo, incluindo cliente Web |
| P1 | IDs inexistentes, mês 0/13, ano fora do intervalo, tipo textual/número inválido, corpo inválido | 400/404 estáveis em vez de 500/200 indevidos |
| P1 | Categoria inexistente/tipo incompatível e exclusão com transações | Integridade relacional e preservação de histórico |
| P1 | Recorrente false/true; 12 parcelas, 31/jan, fevereiro bissexto, dezembro; falha/cancelamento durante gravação | Datas corretas e tudo-ou-nada, sem transação parcial |
| P2 | Precisão de valores, serialização DTO/$values, edição preservando Recorrente, retorno de erros na Web | Contrato e valores financeiros previsíveis |
| P2 | Migrations em banco vazio, upgrade e execução em CI | Reprodutibilidade fora da máquina atual |

Comece com xUnit e integração HTTP via WebApplicationFactory, adaptando a exposição de Program para testes. Use SQL Server descartável para constraints, migrations e transações; um mock não prova comportamento do banco, e EF InMemory não substitui banco relacional. Unitários podem cobrir validação e geração de parcelas. Uma suíte pequena que reproduz os defeitos observados vale mais que cobertura percentual alta com asserts triviais.

## Três melhorias de maior impacto

Estimativas para implementação focada, com revisão; um júnior aprendendo a infraestrutura pode precisar de mais tempo.

| Ordem | Melhoria | Benefício | Esforço | Como verificar |
|---|---|---|---|---|
| 1 | Fechar autorização e unificar a identidade do fluxo Web/API: política obrigatória nos endpoints financeiros, vínculo por usuário e filtros de dono; decidir política das categorias | Impede acesso anônimo e cruzado a dados financeiros. Ensina a diferença entre autenticar quem chama e autorizar o recurso acessado | Alto, 3–5 dias, incluindo migration e integração do login. Revisar atribuição dos dados existentes | Testes de integração anônimo/A/B nos quatro verbos, login real e listagem isolada; usuário B nunca altera dados de A |
| 2 | Corrigir o fluxo financeiro com testes de regressão: rotas Web/API, await, validação, 400/404, persistência atômica de recorrência e precisão monetária | Faz receita/despesa funcionar de ponta a ponta e evita erros silenciosos ou dados parciais | Médio/alto, 3–5 dias; dividir em correções pequenas, deixando migração monetária em etapa própria | Reproduzir os HTTPs deste relatório e mudar os esperados para 400/404; CRUD pela Web; SQL descartável para rollback, datas e centavos |
| 3 | Tornar o desenvolvimento reproduzível: adotar AGENTS, README operacional, SDK/ferramentas fixados, atualizar dependências vulneráveis, definir UI principal, limpar rastreamento de gerados e adicionar CI | Reduz diferenças entre máquinas e dá feedback confiável para trabalho assistido; evita aceitar “test passou” sem testes | Médio, 2–3 dias, mais eventual ajuste de incompatibilidades de pacotes | Clone limpo restaura/compila/testa com testes realmente descobertos; auditoria sem avisos altos pendentes; execução documentada; git status não recebe novos artefatos gerados |

A proposta revisável está em `docs/AGENTS.proposto.md`. Ela descreve o estado atual e as regras de trabalho; sua adoção não corrige automaticamente os defeitos encontrados.
