# LazyCrud.Templates
Pacote contendo templates T4 (.tt) para geração de camadas (Domain, Application, Infra, IoC, UI).

## Instalação
Adicionar ao projeto:
```
<PackageReference Include="LazyCrud.Templates" Version="1.0.0" PrivateAssets="all" />
```
Templates aparecerão como links em `LazyCrudTemplates/`.

## Geração
- Visual Studio: Transform All Templates ou botão da extensão.
- Saída em pasta `T4/` conforme cada template.

## Configuração
- Desativar links: `<UseLazyCrudTemplates>false</UseLazyCrudTemplates>`.
- Copiar fisicamente: `<CopyLazyCrudTemplates>true</CopyLazyCrudTemplates>`.

## Customização
- Usar partial classes para lógica.
- Não editar `.tt` linkado; para alteração profunda, criar fork ou copiar fisicamente.

## Atualização
Atualizar pacote e regenerar.
