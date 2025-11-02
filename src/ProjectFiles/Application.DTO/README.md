# Camada Application.DTO

Visão
- DTOs de Requests/Listining/Steps e Validadores.

Templates
- `DefaultCommands.tt` → inclui: `RequestsDTO.tt`, `ListiningsDTO.tt`, `SteppableRequestsValidators.tt`, `SteppableRequestsDTO.tt`

Entrada
- Entidades/propriedades e atributos (`DisplayOnList`, `Step`, etc.)

Saída
- `T4/<Agg>.*.cs` e stubs em `Aggregates/<Agg>/(Requests|Validators)`

Execução
- VS Run Custom Tool

Links
- Índice: `../README.md`
- Núcleo: `../project.tt`
