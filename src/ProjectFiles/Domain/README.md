# Camada Domain

Visão
- Entidades, VOs, especificações e modelos de consulta por agregado.

Templates
- `DefaultEntities.tt` → inclui `AggregateSettingsEntity.tt`, `UserEntity.tt`, `DefaultFolders.tt`
- `Filters.tt` → wiring de filtros para Specifications
- `Specifications.tt`/`SpecificationItem.tt`/`SpecificationSubItem.tt` → Specifications detalhadas. TODO
- `QueryModels.tt` → modelos de consulta. TODO
- `ProfilesMapping.tt`/`ProfilesListiningMapping.tt` → Profiles. TODO
- `DomainCommandModels.tt`/`DomainCommandHandlers.tt`/`DomainEventModels.tt` → comandos/eventos. TODO
- `CSVMappingProfiles.tt`, `IRespositories.tt`, `ValueObjectsDefaultCommands.tt` → util. TODO

Entrada
- Entidades em `...Domain.Aggregates.<Agg>Agg.Entities`
- Atributos: `EndpointsT4`, `Steppable`, `OneToOne`, `jsonb`, `DisplayOnList`, `Title`, etc.

Saída
- `T4/<Agg>.*.cs` nos projetos Domain

Execução
- VS Run Custom Tool; TransformAll quando disponível

Links
- Índice: `../README.md`
- Núcleo: `../project.tt`
