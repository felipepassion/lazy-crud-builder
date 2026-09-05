# Lazy.Crud.Templates (deprecated alias)

This package is a **dependency-only alias** of `Lazy.Crud.Builder.Templates`, the canonical Lazy CRUD
templates package. It carries no templates and no code.

- If your `lazy.settings` or project references `Lazy.Crud.Templates`, switch to
  `Lazy.Crud.Builder.Templates`. The platform descriptor (`lazycrud.platform.json`) lists
  `Lazy.Crud.Templates` under `templates.aliasPackageIds`, and tooling reports a deprecation notice
  when the alias is used.
- Removal milestone: two minor releases after 1.1.0.

See https://github.com/felipepassion/lazy-crud-builder for the platform documentation.
