# LazyCrud CLI - Design Specification

## Visão Geral

O `lazycrud` CLI será a ferramenta principal para gerenciar aggregates no LazyCrudBuilder.

## Comandos Principais

### 1. `lazycrud init`

Inicializa um novo projeto LazyCrud.

```bash
lazycrud init --name MyProject --template Default

# Flags:
#   --name <string>       Nome do projeto
#   --template <string>   Template base (Default, Minimal, Full)
#   --local               Usa templates locais (default: NuGet)
#   --path <string>       Caminho para criar o projeto
```

**Output:**
- Cria estrutura de diretórios
- Gera `lazy.settings` com configs padrão
- Cria README.md e .gitignore
- Inicializa solution (.sln)

---

### 2. `lazycrud settings`

Gerencia configurações do projeto.

```bash
# Ver todas as configurações
lazycrud settings list

# Definir template source
lazycrud settings set --templates Local
lazycrud settings set --templates NuGet

# Ver configuração específica
lazycrud settings get TemplatesSource

# Resetar para padrões
lazycrud settings reset
```

**Flags importantes:**
- `--templates <Local|NuGet>` - Define source de templates
- `--path <string>` - Caminho para ProjectFiles (quando Local)

---

### 3. `lazycrud create-aggregate`

Cria um novo aggregate com todas as camadas.

```bash
lazycrud create-aggregate --name Products

# Flags:
#   --name <string>           Nome do aggregate (required)
#   --with-tests              Gera camada de testes (default: true)
#   --layers <csv>            Camadas específicas (Domain,Application,...)
#   --template <string>       Template do aggregate (default: Standard)
#   --add-to-solution         Adiciona à solution (default: true)
```

**Output:**
- Cria diretórios: `src/<Aggregate>/*`
- Gera `.csproj` para cada camada
- Cria wrappers T4 (`.tt` files)
- Adiciona projetos à solution
- Atualiza dependências entre projetos

**Camadas criadas:**
- Domain
- Enumerations
- Application.DTO
- Application
- Infra.Data
- Infra.IoC
- Api
- Api.Queries
- Tests.* (se `--with-tests`)

**Exemplo com opções:**
```bash
# Apenas Domain e Application
lazycrud create-aggregate --name Orders --layers Domain,Application

# Sem testes
lazycrud create-aggregate --name Categories --with-tests=false

# Com template customizado
lazycrud create-aggregate --name Inventory --template Microservice
```

---

### 4. `lazycrud delete-aggregate`

Remove um aggregate completamente.

```bash
lazycrud delete-aggregate --name Products

# Flags:
#   --name <string>     Nome do aggregate (required)
#   --hard              Remove físico + refs na solution (default: false)
#   --soft              Apenas remove da solution (mantém arquivos)
#   --backup            Cria backup antes de deletar
```

**Output (--hard):**
- Remove `src/<Aggregate>/`
- Remove projetos da solution
- Remove referências em outros projetos (se houver)
- Opcional: cria backup em `.backups/<Aggregate>_<timestamp>`

**Output (--soft):**
- Apenas remove projetos da solution
- Mantém arquivos físicos

---

### 5. `lazycrud run-tt`

Executa T4 templates de um aggregate.

```bash
lazycrud run-tt --aggregate Products

# Flags:
#   --aggregate <string>    Aggregate alvo (ou * para todos)
#   --all-layers            Roda todos os layers
#   --layers <csv>          Layers específicos
#   --method <string>       MSBuild, DotnetT4, VSTextTransform (auto-detect)
#   --tests-only            Apenas camadas de teste
#   --skip-tests            Pula camadas de teste
#   --parallel              Executa layers em paralelo (quando possível)
```

**Exemplos:**
```bash
# Todos os layers de Products
lazycrud run-tt --aggregate Products --all-layers

# Apenas Domain e Application
lazycrud run-tt --aggregate Products --layers Domain,Application

# Todos os aggregates
lazycrud run-tt --aggregate * --all-layers

# Apenas testes
lazycrud run-tt --aggregate Products --tests-only

# Com método específico
lazycrud run-tt --aggregate Products --method MSBuild
```

**Ordem de execução:**
1. Enumerations
2. Domain
3. Application.DTO
4. Application
5. Infra.Data
6. Infra.IoC
7. Api.Queries
8. Api
9. Tests.* (por último)

---

### 6. `lazycrud e2e`

Executa fluxo E2E completo.

```bash
lazycrud e2e local --aggregate Products

# Flags:
#   --aggregate <string>    Aggregate alvo
#   --clean                 Limpa antes de iniciar
#   --skip-tests            Não roda dotnet test no final
#   --skip-build            Não roda dotnet build
#   --verbose               Output detalhado
```

**Fluxo executado:**
1. Configura `lazy.settings` (templates=Local)
2. Delete aggregate (se existir)
3. Create aggregate (com testes)
4. Cria .csproj files
5. Adiciona à solution
6. Cria T4 wrappers
7. Restaura packages
8. Roda T4 templates
9. Build
10. Test

**Output:**
- Logs de cada etapa
- Sumário no final
- Exit code 0 se sucesso

---

### 7. `lazycrud add-entity`

Adiciona uma nova entidade a um aggregate.

```bash
lazycrud add-entity --aggregate Products --name Product

# Flags:
#   --aggregate <string>    Aggregate alvo
#   --name <string>         Nome da entidade
#   --properties <csv>      Propriedades (Name:string,Price:decimal,...)
#   --generate-tests        Gera testes para entidade (default: true)
#   --run-t4                Roda T4 após criar (default: true)
```

**Output:**
- Cria `<Entity>.cs` no Domain
- Adiciona ao DbContext template
- Gera testes básicos
- Opcionalmente roda T4

**Exemplo:**
```bash
lazycrud add-entity \
  --aggregate Products \
  --name Product \
  --properties "Name:string,Description:string,Price:decimal,Stock:int"
```

---

### 8. `lazycrud build`

Build inteligente (aware de T4).

```bash
lazycrud build

# Flags:
#   --aggregate <string>    Build apenas um aggregate
#   --run-t4                Roda T4 antes do build (default: true)
#   --configuration <str>   Debug ou Release (default: Debug)
#   --restore               Roda dotnet restore antes (default: true)
```

**Fluxo:**
1. Detecta mudanças em `.tt` files
2. Roda T4 se necessário
3. `dotnet restore` (se --restore)
4. `dotnet build`

---

### 9. `lazycrud test`

Executa testes com validações extras.

```bash
lazycrud test

# Flags:
#   --aggregate <string>    Testa apenas um aggregate
#   --layer <string>        Testa apenas uma camada
#   --coverage              Gera relatório de cobertura
#   --watch                 Modo watch (reruns em mudanças)
```

---

### 10. `lazycrud validate`

Valida estrutura do projeto.

```bash
lazycrud validate

# Flags:
#   --aggregate <string>    Valida apenas um aggregate
#   --fix                   Tenta corrigir problemas automaticamente
```

**Verificações:**
- Estrutura de diretórios correta
- `.csproj` existem e são válidos
- Projetos na solution
- T4 wrappers apontam para templates corretos
- Dependências entre projetos corretas
- `lazy.settings` válido

---

## Configuração Global

### Arquivo de config

`~/.lazycrud/config.json` (ou `%APPDATA%\lazycrud\config.json` no Windows)

```json
{
  "defaultTemplateSource": "Local",
  "defaultProjectFilesPath": "src/ProjectFiles",
  "defaultTargetFramework": "net10.0",
  "t4TransformMethod": "Auto",
  "verboseLogging": false,
  "colors": true,
  "aggregateDefaults": {
    "generateTests": true,
    "addToSolution": true,
    "layers": ["Domain", "Application.DTO", "Application", "Infra.Data", "Infra.IoC", "Api", "Api.Queries"]
  }
}
```

### Comandos de config global

```bash
# Ver config global
lazycrud config list

# Setar valor
lazycrud config set defaultTemplateSource Local

# Resetar
lazycrud config reset
```

---

## Aliases e Shortcuts

```bash
# Shortcuts comuns
lazycrud ca <name>           # create-aggregate --name <name>
lazycrud da <name>           # delete-aggregate --name <name> --hard
lazycrud rt <aggregate>      # run-tt --aggregate <aggregate> --all-layers
lazycrud e2e <aggregate>     # e2e local --aggregate <aggregate>

# Aliases
lazycrud new <name>          # create-aggregate --name <name>
lazycrud rm <name>           # delete-aggregate --name <name>
lazycrud t4 <aggregate>      # run-tt --aggregate <aggregate>
```

---

## Exit Codes

- `0` - Sucesso
- `1` - Erro genérico
- `2` - Configuração inválida
- `3` - Aggregate não encontrado
- `4` - T4 transformation falhou
- `5` - Build falhou
- `6` - Testes falharam
- `10` - Ferramenta obrigatória não encontrada

---

## Exemplos de Uso Completo

### Criar novo projeto do zero

```bash
# 1. Init
lazycrud init --name MyEcommerce --local

# 2. Criar primeiro aggregate
lazycrud create-aggregate --name Products

# 3. Adicionar entidade
lazycrud add-entity --aggregate Products --name Product \
  --properties "Name:string,Price:decimal"

# 4. Build e test
lazycrud build
lazycrud test

# 5. Adicionar mais aggregates
lazycrud create-aggregate --name Orders
lazycrud create-aggregate --name Customers
```

### Workflow de desenvolvimento

```bash
# 1. Modificar template em src/ProjectFiles/Domain/...
# 2. Rodar T4
lazycrud run-tt --aggregate Products --layers Domain

# 3. Build
lazycrud build --aggregate Products

# 4. Test
lazycrud test --aggregate Products
```

### Reset completo de um aggregate

```bash
# E2E: deleta e recria tudo
lazycrud e2e local --aggregate Products --clean
```

---

## Implementação Sugerida

### Tecnologia
- **.NET Global Tool** (`dotnet tool install -g lazycrud`)
- **System.CommandLine** para parsing
- **Spectre.Console** para UI rica no terminal

### Estrutura de código

```
src/
├── LazyCrud.CLI/
│   ├── Commands/
│   │   ├── InitCommand.cs
│   │   ├── SettingsCommand.cs
│   │   ├── CreateAggregateCommand.cs
│   │   ├── DeleteAggregateCommand.cs
│   │   ├── RunT4Command.cs
│   │   ├── E2ECommand.cs
│   │   └── ...
│   │
│   ├── Services/
│   │   ├── AggregateService.cs
│   │   ├── T4Service.cs
│   │   ├── SolutionService.cs
│   │   ├── SettingsService.cs
│   │   └── ValidationService.cs
│   │
│   ├── Models/
│   │   ├── AggregateDefinition.cs
│   │   ├── LayerDefinition.cs
│   │   └── LazySettings.cs
│   │
│   └── Program.cs
│
└── LazyCrud.Core/
    ├── Templates/
    ├── Generators/
    └── Utilities/
```

---

## Roadmap

### v1.0 (MVP)
- ✅ `init`
- ✅ `settings`
- ✅ `create-aggregate`
- ✅ `delete-aggregate`
- ✅ `run-tt`
- ✅ `e2e`

### v1.1
- ✅ `add-entity`
- ✅ `build`
- ✅ `test`

### v1.2
- ✅ `validate --fix`
- ✅ Interactive mode
- ✅ Config profiles

### v2.0
- ✅ VSIX integration
- ✅ Template marketplace
- ✅ Cloud deployment helpers
- ✅ Scaffolding avançado

---

**Status**: Design / Spec  
**Implementação**: Ver scripts em `/build/` para protótipos  
**Tracking**: GitHub Issues #TBD
