# ProjectFiles – Índice dos templates

Núcleo
- [`project.tt`](project.tt): contrato base do framework. Todos os `.tt` incluem este arquivo.

Camadas
- [Domain](Domain/README.md)
- [Application](Application/README.md)
- [Application.DTO](Application.DTO/README.md)
- [Infrastructure.Data](Infra.Data/README.md)
- [Infrastructure.IoC](Infra.IoC/README.md)
- [Presentation (Apps)](Apps/README.md)

Como rodar
- VS: Run Custom Tool no `.tt`
- TransformAll: `msbuild /t:TransformAll`
- TextTransform: via Developer PowerShell (TODO)

Tabela rápida (exemplos)
- Domain/Filters.tt → entrada: entidades com `EndpointsT4` → saída: `T4/<Agg>.Filters.cs` → usa núcleo (`GetProperties`, `FindType`, `SaveOutputToSubFolder`)
- Application.DTO/RequestsDTO.tt → entrada: entidades/atributos → saída: `T4/<Agg>.RequestsDTO.cs` + stubs → usa `WriteProperty`
- Application/AppServices.tt → saída: `T4/<Agg>.AppServices.cs`
- Infra.Data/IdentityContext.tt → saída: `T4/<Agg>.IdentityContext.cs`
- Apps/ClientPagesGenerator.tt → páginas Blazor por entidade

Links
- Voltar ao núcleo: [`project.tt`](project.tt)
