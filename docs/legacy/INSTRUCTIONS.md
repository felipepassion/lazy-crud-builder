> **Superseded.** This document predates the Lazy CRUD 1.1.0 platform relaunch and is kept under docs/legacy/ for history only. In the 1.1.0 documentation set it is replaced by the [README](../../README.md) and [Architecture](../architecture.md). The commands, paths and scripts it describes are no longer maintained.

# Instru��es do Projeto Lazy CRUD Builder / Extension

## Vis�o Geral
Plataforma low-code para acelerar cria��o de camadas completas (Domain, Application, Infra, API e UI cliente) via gera��o autom�tica de c�digo usando templates T4 (.tt), integrada a uma extens�o de IDE (Visual Studio) e distribu�da atrav�s de pacotes NuGet reutiliz�veis.

Componentes centrais:
1. Templates T4 (.tt) em `src/ProjectFiles` e `src/Testservice` que geram c�digo de dom�nio, comandos, eventos, handlers, DTOs, controllers, p�ginas cliente e infraestrutura.
2. Extens�o (LazyCrudExtension) que oferece janelas e bot�es para orquestrar gera��o e opera��es r�pidas ("low-code assistido").
3. Pacotes NuGet (`Builder.*`, `CrossCutting.*`) que encapsulam padr�es e utilidades para outras solu��es.

## Estrutura Principal
- Builder: aplica��o n�cleo (Application, Domain, Infra.Data, IoC, WebApi, Queries, Validators, DTO).
- CrossCutting: servi�os horizontais (Log, Mail, Utils, Domain abstractions).
- Testservice: sandbox/exemplo gerado para validar templates e fluxo.
- Extension (reposit�rio separado): VSIX para intera��o visual e automa��o.

Targets: `.NET 9` para projetos modernos / servi�os e `.NET Framework 4.7.2` para compatibilidade da extens�o.

## Categorias de Templates (.tt)
Domain: `DefaultEntities`, `AggregateSettingsEntity`, `Commands`, `DomainEventHandlers`, `DomainCommandHandlers`, `Filters`, `DefaultFolders`.
Application: `AppServices`, `IAppServices`, `DefaultCommands`, DTO espec�ficos.
Infra: `IdentityContext`, comandos/reposit�rios padr�es.
IoC: registros e f�bricas (`ClientIoCFactory`).
API / Queries: `Controllers`, `DashboardControllers`.
UI (Apps): componentes b�sicos (`Basic*`), geradores de p�ginas (`ClientPagesGenerator`, `ClientRegisterPages`, `ClientListiningPages`), formul�rios e estilos (`BasicForm.tt`, `BasicForm.css.tt`).
Testservice: r�plicas dos templates para valida��o isolada.

## Fluxo de Gera��o
1. Definir ou ajustar entidades e modelos nos templates Domain.
2. Acionar transforma��o: via "Transform All Templates" ou bot�es da extens�o.
3. C�digo � gerado automaticamente nas pastas dos projetos alvo.
4. Ajustes espec�ficos: usar `partial classes` ou arquivos fora do escopo gerado (evitar editar diretamente artefatos sobrescritos por .tt).
5. Build e testes r�pidos (API + UI).
6. Publica��o de pacotes NuGet conforme necessidade.

## Conven��es
- Prefixo `Default*` para blocos base reutiliz�veis.
- Sufixos `*Handlers`, `*Models` para comandos e eventos.
- Componentes UI com prefixo `Basic*` para indicar reutiliza��o.
- Separa��o clara Domain vs Application (DTO / Services) para manter isolamento.

## Publica��o NuGet
1. Atualizar vers�o (SemVer) nos projetos alvo.
2. Executar build limpo.
3. Gerar `.nupkg` (dotnet pack / nuget pack).
4. Fazer push para feed (NuGet oficial ou interno).
5. Registrar altera��es em changelog (especialmente mudan�as em contratos p�blicos ou templates geradores).

## Boas Pr�ticas
- N�o remover um `.tt` sem mapear depend�ncias (handlers, controllers, UI).
- Complexidade de regra de neg�cio: manter fora do c�digo gerado (usar servi�os/partials).
- Minimizar diverg�ncia manual ? facilita regenera��o segura.
- Documentar novas entidades e componentes UI.
- Versionar mudan�as estruturais dos templates.

## Customiza��o Segura
- Usar `partial` para l�gica adicional.
- Interfaces em CrossCutting para extens�es.
- Inserir condicionais nos templates somente quando n�o houver alternativa externa.

## Passo Inicial R�pido
1. Clonar reposit�rio principal e extens�o (opcional).
2. Revisar templates Domain para entidades iniciais.
3. Transformar todos os templates.
4. Rodar build.
5. Testar endpoints gerados e p�ginas cliente.
6. Publicar pacotes se for reutilizar em outra solu��o.

## Resumo Final
O projeto entrega uma experi�ncia de desenvolvimento acelerado de CRUDs e camadas adjacentes, combinando gera��o T4, automa��o via extens�o Visual Studio e distribui��o de blocos reutiliz�veis por NuGet; tudo focado em reduzir tempo de codifica��o repetitiva mantendo extensibilidade e boas pr�ticas arquiteturais.
