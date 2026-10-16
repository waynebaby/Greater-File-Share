# Greater File Share v2 Documentation

Status: Draft for review. This branch currently contains documentation only.

## Purpose

Define the v2 behavior and technical design before implementation. No v2 application code may be started until the documentation passes its consistency checks and the user explicitly approves the reviewed documents.

## Source Baseline

- Reverse-engineering source: `master` at commit `fb7e33d` (`Update With new MVVM-Sidekick and applied Style`).
- The source branch is read-only for this effort. Do not copy or modify its code on this branch.
- The current `v2` branch began as a documentation-only branch with `.gitattributes` and `.gitignore`.
- Observed legacy behavior and proposed v2 requirements are distinguished in the documents below. A legacy defect is not a behavior to preserve.

## Confirmed Product Constraints

- Target .NET 10 and a Blazor WebAssembly management UI served by the local manager web server; Native AOT is evaluated separately for WASM and native executables.
- The management UI is available only through loopback. The manager and each file-sharing host run as separate processes.
- The manager starts and supervises host child processes. A normal manager shutdown stops the hosts it started.
- The manager and host communicate using line-delimited JSON-RPC 2.0 over stdio. Stdout is reserved for protocol messages; diagnostics go to stderr.
- A host defaults to binding every network interface, including loopback. The user can change the selected interfaces before starting each sharing instance. This default can expose a host on routed, VPN, or otherwise non-LAN interfaces; the UI and documentation must make that exposure visible.
- The management UI supports both a native folder picker and path entry. Both paths receive the same validation.
- The UI validates named profiles before saving them; CLI `server` commands start and stop saved profiles.
- Preserve settings import/export and investigate migration from the legacy settings format.
- Publish for `win-x64`, `win-arm64`, `linux-x64`, `linux-arm64`, `osx-x64`, and `osx-arm64`. Evaluate AOT separately for every RID; use self-contained publishing for an incompatible RID.
- On manager startup, attempt to open the default browser and print the loopback URL for manual access.

## Documents

- [Current State and Use Cases](current-state-and-use-cases.md): source-backed inventory and behavior mapping.
- [Requirements and Acceptance](requirements-and-acceptance.md): requirement IDs, acceptance criteria, and traceability.
- [Architecture](architecture.md): process boundaries, Blazor options, JSON-RPC, lifecycle, and data flow.
- [Publishing and AOT](publishing-and-aot.md): six-RID release matrix and per-target fallback evidence.
- [Security and Testing](security-and-testing.md): network exposure, filesystem boundaries, and verification plan.
- [Decision Log](decisions.md): accepted decisions, open decisions, and evidence required to close them.

## Documentation Gate

This README list is the summary approval gate for the document set. The `security-and-testing.md` checklist is its detailed evidence companion; overlapping items are intentional cross-checks, not a second independent approval process. Both lists must be satisfied before asking for document approval.

Before requesting approval, verify that:

- [x] Every legacy use case is marked as retained, changed, removed, or unresolved; newly requested v2 use cases are listed separately.
- [x] Every v2 requirement has acceptance criteria and at least one planned verification case.
- [x] The process, loopback, all-interface binding, and shutdown rules agree across documents.
- [x] Management isolation is specified as loopback-only with a non-loopback refusal test. Actual runtime behavior belongs to implementation validation.
- [x] Every RID has a documented AOT evaluation method, known framework restrictions, and a self-contained fallback. Actual publish/runtime results belong to the later implementation/release gate, not this pre-implementation document gate.
- [x] Every open decision is linked to affected requirements/tests and marked as an implementation blocker where it prevents a safe decision.
- [x] All local Markdown links resolve and no document has trailing whitespace.
- [x] Recommendations, accepted user decisions, pending decisions, and release-time validation are clearly distinguished.
- [ ] The user reviews and approves the complete document set.

Implementation remains blocked until the user explicitly approves the validated document set.