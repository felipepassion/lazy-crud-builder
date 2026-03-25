# 📚 Documentação do Fluxo de Ambientes e Implantação - Sumário

https://discord.gg/Fz7tWWVp

1. [🌟 Introdução](#introdução)
2. [🚀 Fluxo E2E Automatizado](#fluxo-e2e-automatizado) **← NOVO!**
9. [💡 Tecnologias Usadas](#tecnologias-usadas)
   - [📑 Índice](#índice)
   - [💻 Tecnologias utilizadas](#tecnologias-utilizadas)

## Introdução

![image](https://github.com/felipepassion/lazy-crud-builder/assets/29386600/1c7cb118-e866-4e86-a696-b3c85c89b059)

---

## 🚀 Fluxo E2E Automatizado

### Quickstart - Recriar Products do Zero

```powershell
# Execute este comando para recriar o aggregate Products completamente
.\build\e2e-products.ps1
```

Este fluxo automatizado irá:
1. ✅ Configurar `lazy.settings` para usar templates locais
2. ✅ Deletar aggregate Products (se existir)
3. ✅ Recriar todas as camadas (Domain, Application, Infra, API, Tests)
4. ✅ Criar arquivos .csproj
5. ✅ Adicionar projetos à solution
6. ✅ Criar wrappers T4
7. ✅ Executar templates T4
8. ✅ Build e Test

### 📖 Documentação E2E

- **[QUICKSTART-E2E.md](QUICKSTART-E2E.md)** - Guia rápido de uso
- **[build/README.md](build/README.md)** - Documentação detalhada
- **[build/CLI-SPEC.md](build/CLI-SPEC.md)** - Especificação do CLI futuro
- **[build/VALIDATION-CHECKLIST.md](build/VALIDATION-CHECKLIST.md)** - Checklist de validação

### Scripts Disponíveis

```powershell
# E2E completo
.\build\e2e-products.ps1

# Criar apenas .csproj
.\build\create-csproj-files.ps1 -AggregateName Products

# Executar apenas T4 templates
.\build\run-t4-templates.ps1 -AggregateName Products
```

### Requisitos

- **PowerShell 7+** (pwsh)
- **.NET 10 SDK**
- **MSBuild** ou **dotnet-t4** (para T4 templates)

> 💡 **Dica**: Este fluxo é idempotente - pode ser executado múltiplas vezes!

---

# Tecnologias Usadas

Este arquivo README fornece uma visão geral das tecnologias utilizadas no projeto e destaca suas características e benefícios.

## Índice

1. [Tecnologias utilizadas](#tecnologias-utilizadas)
    - [.NET Core 8.0 + Blazor WebAssembly](#net-core-70--blazor-webassembly)
    - [Arquitetura DDD com Bounded Contexts](#arquitetura-ddd-com-bounded-contexts)
    - [CQRS - leitura MongoDB, Escrita PostgreSQL](#cqrs---leitura-mongodb-escrita-postgresql)
    - [RabbitMQ](#rabbitmq)
    - [Serilog.Seq](#serilogseq)

## Tecnologias utilizadas

### .NET Core 8.0 + Blazor WebAssembly

O projeto utiliza o .NET Core 8.0 e o Blazor WebAssembly. A combinação dessas tecnologias permite a criação de aplicações web que podem ser executadas offline e facilmente exportadas para dispositivos móveis e desktops com o .NET MAUI. Através do WebAssembly, é possível executar o projeto diretamente no navegador como uma aplicação de página única (SPA).

### Arquitetura DDD com Bounded Contexts

A arquitetura DDD (Domain-Driven Design) com Bounded Contexts é aplicada para garantir uma estruturação clara e modular do código. Isso facilita a manutenção e o desenvolvimento contínuo do projeto.

### CQRS - leitura MongoDB, Escrita PostgreSQL

Para a implementação do padrão CQRS (Command Query Responsibility Segregation), o projeto utiliza MongoDB para leitura e PostgreSQL para escrita. Essa abordagem proporciona um desempenho aprimorado e facilita a escalabilidade.

### RabbitMQ

O RabbitMQ é utilizado para gerenciar filas e troca de informações entre micros serviços. Isso garante uma comunicação eficiente e resiliente entre os componentes do sistema.

### Serilog.Seq

O Serilog.Seq é o gerenciador de logs escolhido para o projeto. Ele permite monitorar e analisar eventos de log em tempo real, facilitando a identificação e solução de problemas.
