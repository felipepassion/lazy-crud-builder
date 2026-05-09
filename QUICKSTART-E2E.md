# LazyCrudBuilder - Quickstart E2E

## 🚀 Execução Rápida (TL;DR)

```powershell
# Execute uma vez para criar tudo do zero
.\build\e2e-products.ps1

# Se T4 não rodar automaticamente
.\build\run-t4-templates.ps1 -AggregateName Products

# Build e test
dotnet build
dotnet test
```

**Pronto!** 🎉

---

## 📚 Documentação Completa

- **[QUICKSTART-E2E.md](QUICKSTART-E2E.md)** (este arquivo) - Início rápido
- **[build/README.md](build/README.md)** - Documentação detalhada do E2E
- **[build/CLI-SPEC.md](build/CLI-SPEC.md)** - Especificação do CLI futuro
- **[build/IMPLEMENTATION-SUMMARY.md](build/IMPLEMENTATION-SUMMARY.md)** - Sumário da implementação
- **[build/VALIDATION-CHECKLIST.md](build/VALIDATION-CHECKLIST.md)** - Checklist de validação

---
1. ✅ Configurar `lazy.settings` para templates locais
2. ✅ Deletar aggregate Products (se existir)
3. ✅ Recriar estrutura completa de camadas
4. ✅ Criar arquivos .csproj para todas as camadas
5. ✅ Adicionar projetos à solution
6. ✅ Criar wrappers T4 (.tt files)
7. ✅ Restaurar pacotes NuGet
8. ⚠️ Tentar executar T4 templates (requer ferramentas)
9. ✅ Build e Test

## 📋 Pré-requisitos

### Obrigatório
- **.NET 10 SDK** ou superior
- **PowerShell 7+** (pwsh)

### Recomendado (para T4)
- **Visual Studio 2026** com workload .NET, ou
- **MSBuild** standalone, ou
- **dotnet-t4**: `dotnet tool install -g dotnet-t4`

## 🔧 Scripts Disponíveis

### 1. E2E Completo
```powershell
.\build\e2e-products.ps1
```

### 2. Apenas criar .csproj
```powershell
.\build\create-csproj-files.ps1 -AggregateName Products
```

### 3. Apenas rodar T4
```powershell
.\build\run-t4-templates.ps1 -AggregateName Products
```

### 4. Rodar T4 sem testes
```powershell
.\build\run-t4-templates.ps1 -AggregateName Products -LayersOnly
```

### 5. Rodar T4 com método específico
```powershell
.\build\run-t4-templates.ps1 -AggregateName Products -Method MSBuild
.\build\run-t4-templates.ps1 -AggregateName Products -Method DotnetT4
```

## 📁 Estrutura Criada

Após executar o E2E, você terá:

```
src/
├── Products/
│   ├── Products.Domain/
│   │   ├── Lazy.Crud.Products.Domain.csproj
│   │   └── DomainTest.tt → ../../ProjectFiles/Domain/Domain.tt
│   │
│   ├── Products.Enumerations/
│   │   ├── Lazy.Crud.Products.Enumerations.csproj
│   │   └── EnumerationsTest.tt
│   │
│   ├── Products.Application.DTO/
│   │   ├── Lazy.Crud.Products.Application.DTO.csproj
│   │   └── [T4 files]
│   │
│   ├── Products.Application/
│   ├── Products.Infra.Data/
│   ├── Products.Infra.IoC/
│   ├── Products.Api/
│   ├── Products.Api.Queries/
│   │
│   └── Tests/
│       ├── Lazy.Crud.Products.Tests.Domain/
│       │   ├── Lazy.Crud.Products.Tests.Domain.csproj
│       │   ├── DomainTest.tt → ../../../ProjectFiles/Tests/Domain/Domain.Tests.tt
│       │   ├── Generated/
│       │   ├── Baseline/
│       │   └── Inputs/
│       │
│       ├── Lazy.Crud.Products.Tests.Enumerations/
│       ├── Lazy.Crud.Products.Tests.DTO/
│       ├── Lazy.Crud.Products.Tests.Application/
│       ├── Lazy.Crud.Products.Tests.InfraData/
│       ├── Lazy.Crud.Products.Tests.InfraIoC/
│       ├── Lazy.Crud.Products.Tests.Api/
│       └── Lazy.Crud.Products.Tests.ApiQueries/
│
├── ProjectFiles/           # Templates locais (source)
│   ├── Domain/
│   ├── Application/
│   ├── Tests/
│   └── ...
│
└── Mods/
    └── lazy.settings       # Configuração (atualizado para Local)
```

## 🎯 Fluxo Típico de Desenvolvimento

### 1. Primeira execução (setup inicial)
```powershell
# Cria tudo do zero
.\build\e2e-products.ps1

# Se T4 não rodar automaticamente, execute manualmente:
.\build\run-t4-templates.ps1 -AggregateName Products

# Build e test
dotnet build
dotnet test
```

### 2. Modificar templates
```powershell
# 1. Edite templates em src/ProjectFiles/...
# 2. Rode T4 novamente
.\build\run-t4-templates.ps1 -AggregateName Products

# 3. Build e test
dotnet build
dotnet test
```

### 3. Resetar tudo
```powershell
# Deleta e recria Products completamente
.\build\e2e-products.ps1
```

## ⚙️ Configuração (lazy.settings)

O arquivo `src/Mods/lazy.settings` é atualizado automaticamente com:

```xml
<Setting Name="TemplatesSource">
  <Value>Local</Value>
</Setting>

<Setting Name="UseNugetTemplates">
  <Value>False</Value>
</Setting>

<Setting Name="TemplatesLocalProjectFilesPath">
  <Value>src/ProjectFiles</Value>
</Setting>
```

Para voltar ao NuGet, edite manualmente ou rode seu comando de settings.

## 🧪 Validação

### Verificar se tudo funcionou:
```powershell
# 1. Estrutura criada
Test-Path src\Products\Products.Domain
Test-Path src\Products\Tests\Lazy.Crud.Products.Tests.Domain

# 2. .csproj existem
Get-ChildItem src\Products -Recurse -Filter "*.csproj"

# 3. T4 wrappers existem
Get-ChildItem src\Products -Recurse -Filter "*.tt"

# 4. Projetos na solution
dotnet sln list | Select-String "Products"

# 5. Build funciona
dotnet build --no-restore

# 6. Testes rodam
dotnet test --no-build
```

## 🐛 Troubleshooting

### Erro: "T4 transformation failed"
**Solução**: Instale uma ferramenta T4:
```powershell
# Opção 1: dotnet-t4 (cross-platform)
dotnet tool install -g dotnet-t4

# Opção 2: Visual Studio (Windows)
# Instale Visual Studio com workload ".NET desktop development"
```

### Erro: "dotnet build failed"
**Causa**: T4 templates não foram executados.

**Solução**:
```powershell
.\build\run-t4-templates.ps1 -AggregateName Products
dotnet build
```

### Erro: "Projects not found in solution"
**Solução**: Re-adicione manualmente:
```powershell
dotnet sln add src\Products\**\*.csproj
```

### Testes falhando
**Verificar**:
1. T4 gerou arquivos `.generated.cs`?
   ```powershell
   Get-ChildItem src\Products -Recurse -Filter "*.generated.cs"
   ```

2. Build passou?
   ```powershell
   dotnet build --no-restore
   ```

3. Dependências corretas nos .csproj?

## 🔄 Ordem de Execução T4

Os templates são executados nesta ordem (respeita dependências):

**Production Layers:**
1. Enumerations
2. Domain
3. Application.DTO
4. Application
5. Infra.Data
6. Infra.IoC
7. Api.Queries
8. Api

**Test Layers:**
9. Tests.Enumerations
10. Tests.Domain
11. Tests.DTO
12. Tests.Application
13. Tests.InfraData
14. Tests.InfraIoC
15. Tests.ApiQueries
16. Tests.Api

## 🎓 Próximos Passos

### Para produção:
1. Criar CLI `lazycrud` que encapsula estes scripts
2. Adicionar comandos:
   ```bash
   lazycrud e2e local --agg Products
   lazycrud create-aggregate --agg Products --with-tests
   lazycrud delete-aggregate --agg Products --hard
   lazycrud run-tt --agg Products --all-layers
   lazycrud settings set --templates Local
   ```

### Para este projeto:
1. Criar entidades reais em Products.Domain
2. Decorar com atributos T4 (`[EndpointsT4]`, etc.)
3. Rodar T4 para gerar código
4. Customizar templates em `src/ProjectFiles/` conforme necessário
5. Repetir o ciclo

## 📚 Documentação Adicional

- [Build README](README.md) - Documentação completa do E2E
- [ProjectFiles README](../src/ProjectFiles/README.md) - Documentação dos templates
- [lazy.settings](../src/Mods/lazy.settings) - Arquivo de configuração

## 💡 Dicas

- **Idempotente**: Pode rodar `e2e-products.ps1` múltiplas vezes
- **Incremental**: Use scripts individuais para passos específicos
- **Customize**: Edite `lazy.settings` para mudar padrões
- **Expanda**: Copie scripts para criar outros aggregates

## ✅ Critérios de Sucesso

E2E está funcionando quando:
- ✅ Script roda sem erros
- ✅ Estrutura de diretórios criada
- ✅ .csproj existem e são válidos
- ✅ Projetos adicionados à solution
- ✅ T4 templates executam (geram .generated.cs)
- ✅ `dotnet build` passa
- ✅ `dotnet test` passa (ou pelo menos roda)
- ✅ Pode repetir o processo (idempotente)

---

**Versão**: 1.0.0  
**Última atualização**: $(Get-Date -Format "yyyy-MM-dd")  
**Compatível com**: .NET 10, PowerShell 7+
