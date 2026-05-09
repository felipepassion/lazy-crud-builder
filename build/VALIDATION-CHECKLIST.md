# E2E Validation Checklist

Use este checklist para validar que o fluxo E2E está funcionando corretamente.

## 📋 Pré-Execução

- [ ] PowerShell 7+ instalado (`pwsh --version`)
- [ ] .NET 10 SDK instalado (`dotnet --version`)
- [ ] Git repositório está limpo (ou backup feito)
- [ ] Visual Studio fechado (opcional, mas recomendado)

## 🚀 Execução do E2E

### Passo 1: Executar Script
```powershell
cd C:\Users\felip\source\repos\lazy-crud-builder
.\build\e2e-products.ps1
```

- [ ] Script iniciou sem erros
- [ ] Todas as etapas mostraram output colorido
- [ ] Sem erros fatais (exit code 0)

### Passo 2: Validar Configuração

**Arquivo:** `src/Mods/lazy.settings`

- [ ] Existe
- [ ] Contém `<Setting Name="TemplatesSource">`
- [ ] Valor é `Local`
- [ ] Contém `<Setting Name="UseNugetTemplates">`
- [ ] Valor é `False`
- [ ] Contém `<Setting Name="TemplatesLocalProjectFilesPath">`
- [ ] Valor aponta para `src/ProjectFiles`

**Comando de verificação:**
```powershell
Get-Content src\Mods\lazy.settings | Select-String "TemplatesSource|UseNugetTemplates|ProjectFilesPath"
```

### Passo 3: Validar Estrutura de Diretórios

- [ ] `src/Products/` existe
- [ ] `src/Products/Products.Domain/` existe
- [ ] `src/Products/Products.Enumerations/` existe
- [ ] `src/Products/Products.Application.DTO/` existe
- [ ] `src/Products/Products.Application/` existe
- [ ] `src/Products/Products.Infra.Data/` existe
- [ ] `src/Products/Products.Infra.IoC/` existe
- [ ] `src/Products/Products.Api/` existe
- [ ] `src/Products/Products.Api.Queries/` existe

**Comando de verificação:**
```powershell
Test-Path src\Products\Products.Domain
Test-Path src\Products\Products.Enumerations
Test-Path src\Products\Products.Application.DTO
Test-Path src\Products\Products.Application
Test-Path src\Products\Products.Infra.Data
Test-Path src\Products\Products.Infra.IoC
Test-Path src\Products\Products.Api
Test-Path src\Products\Products.Api.Queries
```

### Passo 4: Validar Projetos de Teste

- [ ] `src/Products/Tests/Lazy.Crud.Products.Tests.Domain/` existe
- [ ] `src/Products/Tests/Lazy.Crud.Products.Tests.Enumerations/` existe
- [ ] `src/Products/Tests/Lazy.Crud.Products.Tests.DTO/` existe
- [ ] `src/Products/Tests/Lazy.Crud.Products.Tests.Application/` existe
- [ ] `src/Products/Tests/Lazy.Crud.Products.Tests.InfraData/` existe
- [ ] `src/Products/Tests/Lazy.Crud.Products.Tests.InfraIoC/` existe
- [ ] `src/Products/Tests/Lazy.Crud.Products.Tests.Api/` existe
- [ ] `src/Products/Tests/Lazy.Crud.Products.Tests.ApiQueries/` existe

**Comando de verificação:**
```powershell
Get-ChildItem src\Products\Tests -Directory | Measure-Object | Select-Object -ExpandProperty Count
# Deve retornar: 8
```

### Passo 5: Validar Arquivos .csproj

Verificar que cada camada tem seu .csproj:

**Comando:**
```powershell
$csprojFiles = Get-ChildItem src\Products -Recurse -Filter "*.csproj"
$csprojFiles.Count  # Deve ser 16 (8 layers + 8 testes)
$csprojFiles | ForEach-Object { $_.Name }
```

**Lista esperada:**
- [ ] Lazy.Crud.Products.Domain.csproj
- [ ] Lazy.Crud.Products.Enumerations.csproj
- [ ] Lazy.Crud.Products.Application.DTO.csproj
- [ ] Lazy.Crud.Products.Application.csproj
- [ ] Lazy.Crud.Products.Infra.Data.csproj
- [ ] Lazy.Crud.Products.Infra.IoC.csproj
- [ ] Lazy.Crud.Products.Api.csproj
- [ ] Lazy.Crud.Products.Api.Queries.csproj
- [ ] Lazy.Crud.Products.Tests.Domain.csproj
- [ ] Lazy.Crud.Products.Tests.Enumerations.csproj
- [ ] Lazy.Crud.Products.Tests.DTO.csproj
- [ ] Lazy.Crud.Products.Tests.Application.csproj
- [ ] Lazy.Crud.Products.Tests.InfraData.csproj
- [ ] Lazy.Crud.Products.Tests.InfraIoC.csproj
- [ ] Lazy.Crud.Products.Tests.Api.csproj
- [ ] Lazy.Crud.Products.Tests.ApiQueries.csproj

### Passo 6: Validar Arquivos T4 (.tt)

**Comando:**
```powershell
$ttFiles = Get-ChildItem src\Products -Recurse -Filter "*.tt"
$ttFiles.Count
$ttFiles | ForEach-Object { Write-Host "$($_.Directory.Name) -> $($_.Name)" }
```

Para cada .tt encontrado, verificar que:
- [ ] Contém `<#@ include file="...">` 
- [ ] Aponta para `../../ProjectFiles/...` ou `../../../ProjectFiles/...`
- [ ] Path do include está correto

**Exemplo de verificação manual:**
```powershell
Get-Content src\Products\Products.Domain\DomainTest.tt
# Deve mostrar: <#@ include file="..\..\ProjectFiles\Domain\Domain.tt" #>
```

### Passo 7: Validar Solution

**Comando:**
```powershell
dotnet sln list | Select-String "Products"
```

- [ ] Todos os 16 projetos aparecem
- [ ] Nenhum projeto duplicado
- [ ] Paths estão corretos (src\Products\...)

### Passo 8: Restaurar Pacotes

```powershell
dotnet restore
```

- [ ] Restore completou com sucesso
- [ ] Nenhum erro de packages não encontrados
- [ ] Warnings são aceitáveis

### Passo 9: Executar T4 Templates

```powershell
.\build\run-t4-templates.ps1 -AggregateName Products
```

- [ ] Script detectou ferramenta T4 (MSBuild, dotnet-t4, ou TextTransform)
- [ ] Processou todas as camadas em ordem
- [ ] Nenhum erro fatal
- [ ] Output mostra "✓ completed" para cada layer

**Se falhar:** Instale ferramenta T4:
```powershell
dotnet tool install -g dotnet-t4
```

### Passo 10: Verificar Arquivos Gerados

Após rodar T4, verificar que arquivos `.generated.cs` foram criados:

```powershell
Get-ChildItem src\Products -Recurse -Filter "*.generated.cs" | Measure-Object
# Deve ter > 0 arquivos
```

Para cada arquivo gerado:
- [ ] Contém `// <auto-generated>`
- [ ] Contém código C# válido
- [ ] Não está vazio

**Exemplo:**
```powershell
$generated = Get-ChildItem src\Products -Recurse -Filter "*.generated.cs" | Select-Object -First 1
Get-Content $generated.FullName | Select-Object -First 10
# Deve mostrar header com <auto-generated>
```

### Passo 11: Build

```powershell
dotnet build --no-restore
```

- [ ] Build completou com sucesso
- [ ] 0 errors
- [ ] Warnings são aceitáveis (se houver, revisar)
- [ ] Todos os 16 projetos buildaram

**Verificar output:**
```
Build succeeded.
    0 Warning(s)
    0 Error(s)
```

### Passo 12: Executar Testes

```powershell
dotnet test --no-build --verbosity normal
```

- [ ] Testes executaram (mesmo que falhem)
- [ ] Pelo menos alguns testes rodaram
- [ ] Nenhum erro de "project not found"

**Output esperado (mínimo):**
```
Test run for ...
Total tests: X
     Passed: Y
     Failed: Z
```

## 🔁 Teste de Idempotência

Execute o E2E novamente para validar idempotência:

```powershell
.\build\e2e-products.ps1
```

**Validações:**
- [ ] Script executou sem erros
- [ ] Deletou Products anterior
- [ ] Recriou tudo novamente
- [ ] Estrutura final é idêntica à primeira execução
- [ ] Nenhum projeto duplicado na solution

## 📊 Validação de Conteúdo

### Verificar .csproj (amostra)

**Arquivo:** `src/Products/Products.Domain/Lazy.Crud.Products.Domain.csproj`

```powershell
Get-Content src\Products\Products.Domain\Lazy.Crud.Products.Domain.csproj
```

Verificar que contém:
- [ ] `<TargetFramework>net10.0</TargetFramework>`
- [ ] `<PackageReference Include="MediatR" .../>`
- [ ] `<PackageReference Include="FluentValidation" .../>`
- [ ] Seção `<ItemGroup>` para T4

### Verificar Test .csproj (amostra)

**Arquivo:** `src/Products/Tests/Lazy.Crud.Products.Tests.Domain/Lazy.Crud.Products.Tests.Domain.csproj`

```powershell
Get-Content src\Products\Tests\Lazy.Crud.Products.Tests.Domain\Lazy.Crud.Products.Tests.Domain.csproj
```

Verificar que contém:
- [ ] `<IsTestProject>true</IsTestProject>`
- [ ] `<PackageReference Include="xunit" .../>`
- [ ] `<PackageReference Include="Microsoft.NET.Test.Sdk" .../>`
- [ ] `<ProjectReference Include="..\..\Products.Domain\..."/>`

### Verificar T4 Wrapper (amostra)

**Arquivo:** `src/Products/Tests/Lazy.Crud.Products.Tests.Domain/DomainTest.tt`

```powershell
Get-Content src\Products\Tests\Lazy.Crud.Products.Tests.Domain\DomainTest.tt
```

Verificar que:
- [ ] É apenas 1 linha: `<#@ include file="..." #>`
- [ ] Path aponta para: `../../../ProjectFiles/Tests/Domain/Domain.Tests.tt`
- [ ] Arquivo include existe e é acessível

## 🧹 Limpeza (Opcional)

Para resetar completamente e testar novamente:

```powershell
# Deletar manualmente
Remove-Item src\Products -Recurse -Force

# Ou executar E2E novamente (ele deleta automaticamente)
.\build\e2e-products.ps1
```

## ✅ Checklist de Sucesso Final

Marque TODAS para considerar E2E validado:

- [ ] ✅ Script E2E executa sem erros
- [ ] ✅ lazy.settings configurado para Local
- [ ] ✅ 8 camadas de produção criadas
- [ ] ✅ 8 camadas de teste criadas
- [ ] ✅ 16 arquivos .csproj existem e são válidos
- [ ] ✅ Todos os projetos adicionados à solution
- [ ] ✅ Arquivos .tt criados e apontam para ProjectFiles
- [ ] ✅ T4 templates executam (com ferramenta disponível)
- [ ] ✅ Arquivos .generated.cs criados
- [ ] ✅ dotnet build passa (0 errors)
- [ ] ✅ dotnet test executa
- [ ] ✅ E2E é idempotente (pode rodar 2x)

## 📝 Notas e Observações

Use este espaço para anotar problemas encontrados ou ajustes necessários:

```
Data: ___________
Testado por: ___________

Observações:
- 
- 
- 

Problemas encontrados:
- 
- 
- 

Ajustes feitos:
- 
- 
- 
```

---

## 🆘 Se algo falhar

### T4 não executou
1. Instale ferramenta:
   ```powershell
   dotnet tool install -g dotnet-t4
   ```
2. Execute manualmente:
   ```powershell
   .\build\run-t4-templates.ps1 -AggregateName Products
   ```

### Build falhou
1. Verifique se T4 foi executado
2. Verifique se todos os .csproj existem
3. Execute:
   ```powershell
   dotnet clean
   dotnet restore
   dotnet build
   ```

### Projetos não aparecem na solution
1. Adicione manualmente:
   ```powershell
   dotnet sln add src\Products\**\*.csproj
   ```

### E2E travou ou interrompido
1. Delete Products manualmente:
   ```powershell
   Remove-Item src\Products -Recurse -Force
   ```
2. Execute E2E novamente

---

**Versão:** 1.0  
**Última atualização:** 2024  
**Compatível com:** LazyCrudBuilder E2E v1.0
