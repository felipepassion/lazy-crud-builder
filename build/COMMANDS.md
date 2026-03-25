# Quick Reference - E2E Commands

## 🚀 Main Commands

### Execute E2E (Complete Flow)
```powershell
.\build\e2e-products.ps1
```

### Create .csproj Files Only
```powershell
.\build\create-csproj-files.ps1 -AggregateName Products
```

### Run T4 Templates Only
```powershell
.\build\run-t4-templates.ps1 -AggregateName Products
```

### Run T4 Without Tests
```powershell
.\build\run-t4-templates.ps1 -AggregateName Products -LayersOnly
```

### Run T4 With Specific Method
```powershell
.\build\run-t4-templates.ps1 -AggregateName Products -Method MSBuild
.\build\run-t4-templates.ps1 -AggregateName Products -Method DotnetT4
```

---

## 🔧 Validation Commands

### Check Structure
```powershell
# List all Products projects
Get-ChildItem src\Products -Recurse -Filter "*.csproj"

# Count projects (should be 16)
(Get-ChildItem src\Products -Recurse -Filter "*.csproj").Count

# List T4 files
Get-ChildItem src\Products -Recurse -Filter "*.tt"

# List generated files
Get-ChildItem src\Products -Recurse -Filter "*.generated.cs"
```

### Check Solution
```powershell
# List Products projects in solution
dotnet sln list | Select-String "Products"

# Count Products projects in solution
(dotnet sln list | Select-String "Products").Count
```

### Check Settings
```powershell
# View lazy.settings
Get-Content src\Mods\lazy.settings

# Check template source
Get-Content src\Mods\lazy.settings | Select-String "TemplatesSource|UseNugetTemplates"
```

---

## 🏗️ Build Commands

### Restore
```powershell
dotnet restore
```

### Build
```powershell
dotnet build
dotnet build --no-restore
dotnet build --no-restore --configuration Release
```

### Clean
```powershell
dotnet clean
```

### Rebuild
```powershell
dotnet clean
dotnet restore
dotnet build
```

---

## 🧪 Test Commands

### Run All Tests
```powershell
dotnet test
```

### Run Tests Without Build
```powershell
dotnet test --no-build
```

### Run Tests With Verbosity
```powershell
dotnet test --verbosity normal
dotnet test --verbosity detailed
```

### Run Specific Test Project
```powershell
dotnet test src\Products\Tests\Lazy.Crud.Products.Tests.Domain\Lazy.Crud.Products.Tests.Domain.csproj
```

### Run Tests With Coverage
```powershell
dotnet test --collect:"XPlat Code Coverage"
```

---

## 🗑️ Cleanup Commands

### Delete Products (Manual)
```powershell
Remove-Item src\Products -Recurse -Force
```

### Remove Products from Solution (Manual)
```powershell
# Get all Products projects
$projects = Get-ChildItem src\Products -Recurse -Filter "*.csproj"

# Remove each from solution
foreach ($proj in $projects) {
    dotnet sln remove $proj.FullName
}
```

### Clean Build Artifacts
```powershell
# Remove bin and obj folders
Get-ChildItem src\Products -Recurse -Directory | Where-Object { $_.Name -eq "bin" -or $_.Name -eq "obj" } | Remove-Item -Recurse -Force
```

---

## 📦 Solution Management

### Add All Products Projects
```powershell
dotnet sln add src\Products\**\*.csproj
```

### List All Projects
```powershell
dotnet sln list
```

### Create New Solution
```powershell
dotnet new sln -n LazyCrudBuilder
```

---

## 🔍 Inspection Commands

### Count Files by Type
```powershell
# .csproj files
(Get-ChildItem src\Products -Recurse -Filter "*.csproj").Count

# .tt files
(Get-ChildItem src\Products -Recurse -Filter "*.tt").Count

# .cs files
(Get-ChildItem src\Products -Recurse -Filter "*.cs").Count

# .generated.cs files
(Get-ChildItem src\Products -Recurse -Filter "*.generated.cs").Count
```

### Find Files With Content
```powershell
# Find .tt files with specific include
Get-ChildItem src\Products -Recurse -Filter "*.tt" | ForEach-Object {
    $content = Get-Content $_.FullName
    if ($content -match "ProjectFiles") {
        Write-Host "$($_.FullName): $content"
    }
}
```

### Check T4 Template Paths
```powershell
Get-ChildItem src\Products -Recurse -Filter "*.tt" | ForEach-Object {
    Write-Host "`n$($_.FullName)"
    Get-Content $_.FullName
}
```

---

## 🛠️ T4 Tools

### Install dotnet-t4
```powershell
dotnet tool install -g dotnet-t4
```

### Update dotnet-t4
```powershell
dotnet tool update -g dotnet-t4
```

### Check dotnet-t4 Version
```powershell
dotnet t4 --version
```

### Run Single T4 File
```powershell
cd src\Products\Products.Domain
dotnet t4 DomainTest.tt
```

---

## 📊 Reporting Commands

### Project Statistics
```powershell
Write-Host "=== Products Aggregate Statistics ===" -ForegroundColor Cyan
Write-Host "Projects:           $((Get-ChildItem src\Products -Recurse -Filter '*.csproj').Count)"
Write-Host "T4 Templates:       $((Get-ChildItem src\Products -Recurse -Filter '*.tt').Count)"
Write-Host "Generated Files:    $((Get-ChildItem src\Products -Recurse -Filter '*.generated.cs').Count)"
Write-Host "Total C# Files:     $((Get-ChildItem src\Products -Recurse -Filter '*.cs').Count)"
Write-Host "Test Projects:      $((Get-ChildItem src\Products\Tests -Recurse -Filter '*.csproj').Count)"
```

### Build Summary
```powershell
dotnet build --no-restore | Select-String "Build succeeded|Build FAILED|Error\(s\)|Warning\(s\)"
```

---

## 🔄 Common Workflows

### Workflow 1: Fresh Start
```powershell
# 1. Run E2E
.\build\e2e-products.ps1

# 2. Run T4 (if not auto-run)
.\build\run-t4-templates.ps1 -AggregateName Products

# 3. Build
dotnet build

# 4. Test
dotnet test
```

### Workflow 2: Template Changes
```powershell
# 1. Edit template in src/ProjectFiles/...
# (manual step)

# 2. Re-run T4
.\build\run-t4-templates.ps1 -AggregateName Products

# 3. Rebuild
dotnet build

# 4. Test
dotnet test
```

### Workflow 3: Reset Everything
```powershell
# 1. Clean
dotnet clean

# 2. Run E2E
.\build\e2e-products.ps1

# 3. Run T4
.\build\run-t4-templates.ps1 -AggregateName Products

# 4. Build & Test
dotnet build
dotnet test
```

### Workflow 4: Verify Installation
```powershell
# 1. Check structure
Test-Path src\Products\Products.Domain

# 2. Count projects
(Get-ChildItem src\Products -Recurse -Filter "*.csproj").Count

# 3. Check solution
dotnet sln list | Select-String "Products"

# 4. Try build
dotnet build --no-restore
```

---

## 🎯 Troubleshooting Commands

### T4 Not Working?
```powershell
# Check if MSBuild is available
Get-Command msbuild

# Check if dotnet-t4 is installed
dotnet tool list -g | Select-String "dotnet-t4"

# Install dotnet-t4
dotnet tool install -g dotnet-t4
```

### Build Failing?
```powershell
# Clean everything
dotnet clean

# Remove bin/obj
Get-ChildItem -Recurse -Directory | Where-Object { $_.Name -eq "bin" -or $_.Name -eq "obj" } | Remove-Item -Recurse -Force

# Restore and rebuild
dotnet restore
dotnet build
```

### Projects Not in Solution?
```powershell
# Re-add all Products projects
dotnet sln add src\Products\**\*.csproj

# Verify
dotnet sln list | Select-String "Products"
```

---

## 📚 Documentation Commands

### Open Documentation
```powershell
# Windows
Start-Process QUICKSTART-E2E.md
Start-Process build\README.md

# Cross-platform (if code/VSCode available)
code QUICKSTART-E2E.md
code build\README.md
```

### Generate Markdown TOC
```powershell
# List all documentation
Get-ChildItem -Recurse -Filter "*.md" | Select-Object FullName
```

---

## 💡 Tips

### Set Alias for Quick Access
```powershell
# In your PowerShell profile
Set-Alias e2e ".\build\e2e-products.ps1"
Set-Alias t4 ".\build\run-t4-templates.ps1"

# Usage:
# e2e
# t4 -AggregateName Products
```

### Watch for Changes
```powershell
# Monitor file changes
Get-ChildItem src\ProjectFiles -Recurse -Filter "*.tt" | ForEach-Object {
    $watcher = New-Object System.IO.FileSystemWatcher
    $watcher.Path = $_.DirectoryName
    $watcher.Filter = $_.Name
    $watcher.EnableRaisingEvents = $true
    Register-ObjectEvent $watcher "Changed" -Action {
        Write-Host "Template changed, re-running T4..." -ForegroundColor Yellow
        .\build\run-t4-templates.ps1 -AggregateName Products
    }
}
```

---

**Last Updated:** 2024  
**Compatibility:** PowerShell 7+, .NET 10+
