# Camada Infrastructure.Data

Visão
- Persistência EF Core: DbContext, Mappings e Repositórios.

Templates
- `DefaultCommands.tt` → inclui `IdentityContext.tt`, `Mappings.tt`, `Respositories.tt`

Entrada
- Entidades e atributos relacionais

Saída
- `T4/<Agg>.IdentityContext.cs`, `T4/<Agg>.Mappings.cs`, `T4/<Agg>.Respositories.cs`

Execução
- VS Run Custom Tool

Links
- Índice: `../README.md`
- Núcleo: `../project.tt`
