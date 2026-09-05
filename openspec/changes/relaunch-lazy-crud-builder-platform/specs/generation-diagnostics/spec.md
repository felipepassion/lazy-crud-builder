## ADDED Requirements

### Requirement: No silent failures in the generation path

The generation pipeline SHALL NOT discard exceptions. Every failure in writing output, resolving templates, resolving the model or persisting state SHALL produce a diagnostic that reaches the caller.

#### Scenario: Failed file write is reported

- **WHEN** writing a generated file fails, for example because the file is locked or the path is not writable
- **THEN** a diagnostic identifies the target path and the underlying error
- **AND** the generation run reports failure
- **AND** the run does not report the file as generated

#### Scenario: Directory creation failure is reported

- **WHEN** creating the output directory fails
- **THEN** a diagnostic identifies the directory and the underlying error
- **AND** the run reports failure

#### Scenario: Empty catch blocks are removed from the write path

- **WHEN** the generation write path is inspected
- **THEN** no exception handler discards its exception without producing a diagnostic
- **AND** a repository check asserts this for the shared writer

#### Scenario: Model resolution failure is visible

- **WHEN** a type or project entry in the model cannot be resolved
- **THEN** the failure is recorded as a diagnostic naming the unresolved item
- **AND** the run's outcome reflects whether the failure was fatal or tolerated

### Requirement: Diagnostics are structured and actionable

Each diagnostic SHALL carry a stable code, a severity, the affected path or item, and a message stating what to do next.

#### Scenario: Diagnostic carries a stable code

- **WHEN** any diagnostic is emitted
- **THEN** it has a stable, documented code
- **AND** tests and tooling can assert on the code rather than message text

#### Scenario: Severity distinguishes fatal from tolerated

- **WHEN** diagnostics are emitted during a run
- **THEN** each is classified as error, warning or information
- **AND** the run fails if and only if at least one error was emitted

#### Scenario: Message states the remedy

- **WHEN** an error diagnostic is emitted
- **THEN** its message names the affected file or item and the action that resolves it

### Requirement: Diagnostics are testable

Diagnostics SHALL be observable by automated tests without parsing IDE output.

#### Scenario: Diagnostics are collected programmatically

- **WHEN** a generation run completes
- **THEN** the emitted diagnostics are available as structured data
- **AND** a unit test can assert on codes, severities and affected paths

#### Scenario: Machine-readable diagnostics for headless runs

- **WHEN** generation is invoked headlessly
- **THEN** diagnostics are written to a machine-readable file
- **AND** an agent or CI job can determine the outcome without reading console text

### Requirement: Template resolution failures are explicit

Failure to locate templates SHALL be reported rather than masked by a fallback.

#### Scenario: Missing templates are reported

- **WHEN** the configured template source cannot be resolved
- **THEN** a diagnostic names the configured source, the package id or path attempted, and each location probed
- **AND** the run does not silently proceed with a different source

#### Scenario: Package resolution never degrades to local silently

- **WHEN** package-based template resolution fails
- **THEN** the run fails with the resolution diagnostic
- **AND** it does not switch to a local source tree

#### Scenario: The resolved source is always reported

- **WHEN** any generation run completes
- **THEN** the report names the template source kind, its location and the resolved version
- **AND** an explicitly selected local development mode is reported as such
