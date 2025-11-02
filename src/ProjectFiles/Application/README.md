# Camada Application

Visão
- Serviços de aplicação por entidade, expondo operações com base em `EndpointsT4`.

Templates
- `DefaultCommands.tt` → inclui `AppServices.tt` e `IAppServices.tt`
- `AppServices.tt` → classes `*AppService`
- `IAppServices.tt` → interfaces `I*AppService`

Entrada
- Entidades com `[EndpointsT4]`, `Filters` e DTOs

Saída
- `T4/<Agg>.AppServices.cs`, `T4/<Agg>.IAppServices.cs`

Execução
- VS Run Custom Tool

Links
- Índice: `../README.md`
- Núcleo: `../project.tt`
