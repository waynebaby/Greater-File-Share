# Requirements and Acceptance

This document is the requirements source of truth for v2. It separates confirmed decisions from items that must be resolved by architecture evidence or explicit review. `UC-*` references point to [Current State and Use Cases](current-state-and-use-cases.md).

## Functional Requirements

| ID | Requirement | Acceptance criteria | Traces |
| --- | --- | --- | --- |
| FR-01 | Provide a Blazor management interface from the .NET 10 manager executable. | A fresh launch presents or prints a usable loopback URL; the selected render mode and AOT implications are justified in the architecture record. | UC-01 |
| FR-02 | Keep management access loopback-only. | Requests to management pages and control endpoints succeed through loopback and fail through every non-loopback interface, including the host's LAN IP. | UC-01, UC-03 |
| FR-03 | Manage separate file-host child processes. | Manager can start, stop, observe, and report failure for each instance; a normal manager shutdown stops every child it owns. | UC-02, UC-03 |
| FR-04 | Use line-delimited JSON-RPC 2.0 over stdio between manager and host. | stdout contains protocol frames only; stderr may contain diagnostics; malformed frames, child exit, timeout, and cancellation produce a visible failed/stopped state. | UC-03, UC-08 |
| FR-05 | Create, select, order, and remove multiple share instances. | Each instance has a stable identity and independent root, port, content types, and interface selection. Reordering changes display order only. | UC-02, UC-04 |
| FR-06 | Choose a share root with either a native folder picker or typed/pasted path. | Both entry paths use the same canonicalization, existence, permission, and containment checks; cancellation does not corrupt the current value. Picker compatibility/fallback is pending O-001; path entry is not an approved replacement for the native picker. | UC-02 |
| FR-07 | Serve files and directory listings from the configured share root. | Clients can browse directories and download permitted files over HTTP; traversal, sibling-prefix paths, and link/reparse-point escapes cannot leave the configured root. Link traversal policy is pending O-011. | UC-05, UC-10 |
| FR-08 | Configure a listening port and extension-to-MIME mappings. | Invalid ports, duplicate extensions, malformed MIME values, and overlapping bind tuples yield actionable feedback without marking the host healthy. Port 0 and OS-assigned ports remain pending O-006. | UC-02, UC-07 |
| FR-09 | Bind selected network interfaces per instance. | Default selection includes every local interface including loopback; the user can adjust the selected interfaces before starting each instance. The UI displays exactly which addresses will be reachable. | UC-05 |
| FR-10 | Provide direct links and QR codes. | Links are generated for a share root, selected folder/file, and optional short link; custom URL QR generation remains available. Escaping and IPv6 formatting are correct. | UC-05, UC-06 |
| FR-11 | Decide which legacy API/catalog behavior to preserve, replace, or remove. | Before implementation approval, O-004 records each retained/replaced/removed HTTP endpoint and WCF operation, its route/schema, error behavior, and security boundary. Until then, UC-10/11 and AT-09 are blocked from implementation sign-off. | UC-10, UC-11 |
| FR-12 | Save and exchange settings. | Users can save, import, and export versioned configuration. Import validates the whole document before replacing live state and has a documented recovery path. Exports must not contain credentials in plaintext; credential treatment depends on O-002. | UC-09 |
| FR-13 | Migrate legacy configuration. | After O-005 identifies representative `.gfssetting` fixtures and supported variants, each confirmed format migrates without losing supported fields; unsupported fields are reported and the source file remains unchanged. | UC-09 |
| FR-14 | Surface useful logs and per-instance status. | Start/stop, bind addresses, port failures, child crashes, and import/migration errors are visible with instance correlation. Sensitive local paths are not sent to share clients. | UC-03, UC-08 |
| FR-15 | Validate and save named profiles in the UI. | Validation is side-effect-free. Provisional blocking cases are invalid schema, missing/inaccessible root, invalid port range or conflicting bind tuple, duplicate stable IDs, and malformed MIME; provisional warnings are currently unavailable selected interfaces and exposure beyond a private LAN. The saved profile has stable identity and a versioned ordered instance list. Final schema and severity rules are pending O-009. | UC-15 |
| FR-16 | Start and stop a saved profile from the command line. | `server` commands start/stop a named saved profile through the same manager/orchestration path as the UI, report per-instance status/URLs, return nonzero on failure, and expose no management operation on share listeners. Exact grammar and manager lifetime remain pending O-010. | UC-16 |

## Cross-Cutting Requirements

| ID | Requirement | Acceptance criteria |
| --- | --- | --- |
| NFR-01 | Target .NET 10 and six RIDs. | Release matrix covers Windows, Linux, and macOS for x64 and arm64; each artifact launches on the matching OS/architecture. |
| NFR-02 | Evaluate Native AOT per RID. | Each RID records AOT build result, warnings, runtime smoke test, known limitations, or the reason for self-contained fallback. |
| NFR-03 | Publish self-contained when AOT is not viable. | Every fallback artifact includes the runtime and passes the same functional smoke suite as AOT artifacts. |
| NFR-04 | Keep share access separate from management access. | Host routes contain no management operations; management routes cannot be reached over a selected sharing interface. |
| NFR-05 | Make exposure explicit. | Before start, show the effective address list, including loopback and any routed/VPN interfaces selected. Deselecting an interface prevents binding and sharing links for that address. |
| NFR-06 | Prevent unexpected sharing after restart or migration. | Interim default: no host autostarts after manager restart or settings migration until O-003 is approved. Any later autostart must be an explicit per-profile opt-in and visible in the UI. |

## Acceptance Test IDs

| Test ID | Scope | Minimum assertion |
| --- | --- | --- |
| AT-01 | Manager launch | Default browser launch is attempted and the loopback URL is printed; no share starts without an explicitly selected root and start action. |
| AT-02 | Admin isolation | IPv4 and IPv6 loopback succeed when available; requests to all non-loopback addresses are refused. |
| AT-03 | Child protocol | JSON-RPC request/response works and stdout stays parseable; malformed/partial frames, invalid UTF-8, broken pipe, child crash, cancellation, and shutdown timeout produce visible errors. |
| AT-04 | Lifecycle | Start/stop status is correct; manager shutdown stops owned children. |
| AT-05 | Interface binding | For every selected address, the host accepts requests; for each deselected address, it does not. Include loopback, IPv4, IPv6, and multiple adapters where available. |
| AT-06 | File containment | Normal nested paths work; traversal, sibling-prefix, encoded separator, case variant, and symlink/reparse-point escape attempts are rejected. In-root link behavior follows the O-011 policy only after that decision is approved. |
| AT-07 | Share UX | Folder picker, path input, MIME validation, QR, copyable URLs, and error messages work. |
| AT-08 | Settings | After O-005 defines the supported legacy fixtures, migration works; invalid import leaves current settings/source intact; export can be re-imported without plaintext credentials. |
| AT-09 | HTTP behavior | After O-004 approves the endpoint matrix, directory listing, file download, content types, selected API routes, and short-link behavior match that contract. |
| AT-10 | RID matrix | AOT or self-contained package for every RID launches and passes platform smoke tests. |
| AT-11 | Profile validation | Under the provisional O-009 rules, blocking cases prevent runnable save, warnings remain visible, validation starts no process, and a valid profile saves/reloads without loss. Final cases follow approved O-009. |
| AT-12 | CLI profile lifecycle | Start a named profile from CLI, verify each host and URL, stop that profile from a separate CLI invocation, verify only its hosts stop, and test unknown profile/already-stopped/start-failure exit codes. |
| AT-13 | CLI manager lifecycle | If O-010 approves the recommended daemon lifecycle, verify CLI start reuses/starts the headless manager, profile stop leaves it available, and explicit shutdown stops all managed hosts and exits. |
| AT-14 | First-run configuration | Until O-007 is approved, the manager presents an unshared profile requiring an explicit root selection; no OS-specific directory is silently exposed. |
| AT-15 | Logs and task status | Start/stop, bind failures, child crashes, import failures, and profile validation results are visible with the correct instance ID and without exposing absolute roots to share clients. |
| AT-16 | Restart and migration sharing safety | After O-005 identifies a fixture carrying the legacy hosting flag, restart or migrate settings and verify no share host starts until the user explicitly starts a profile; verify legacy `IsSetToHosting` does not silently start a v2 host. |

## Requirement Traceability Matrix

Every functional and cross-cutting requirement maps to a user flow, at least one acceptance test, and its controlling design section or pending decision. A requirement tied to an open decision is not implementation-ready until that decision is approved.

| Requirement | Use case(s) | Acceptance test(s) | Design section / decision |
| --- | --- | --- | --- |
| FR-01 | UC-01 | AT-01 | Architecture: Mainline; Publishing and AOT |
| FR-02 | UC-01, UC-03 | AT-02 | Security: Management surface; Architecture: Manager |
| FR-03 | UC-02, UC-03 | AT-04 | Architecture: Process responsibilities and Host lifecycle |
| FR-04 | UC-03, UC-08 | AT-03 | Architecture: Control protocol |
| FR-05 | UC-02, UC-04 | AT-04, AT-08, AT-11 | Architecture: Configuration and Migration; O-009 |
| FR-06 | UC-02 | AT-07 | Architecture: Mainline; O-001 |
| FR-07 | UC-05, UC-10 | AT-06, AT-09 | Architecture: Share host; Security: Filesystem boundary; O-004, O-011 |
| FR-08 | UC-02, UC-07 | AT-05, AT-09 | Architecture: Network and URL Model; O-006 |
| FR-09 | UC-05 | AT-05 | Architecture: Network and URL Model |
| FR-10 | UC-05, UC-06 | AT-07, AT-09 | Architecture: Network and URL Model |
| FR-11 | UC-10, UC-11 | AT-09 | Decisions: O-004 (required before implementation approval) |
| FR-12 | UC-09 | AT-08 | Architecture: Configuration and Migration; Security: Settings and logs; O-002 |
| FR-13 | UC-09 | AT-08 | Architecture: Configuration and Migration; O-005 (fixture set required) |
| FR-14 | UC-03, UC-08 | AT-04, AT-15 | Architecture: Logging and Status; Security: Settings and logs |
| FR-15 | UC-15 | AT-11 | Architecture: Configuration and Migration; O-009 (schema/severity approval) |
| FR-16 | UC-16 | AT-12, AT-13 | Architecture: CLI lifecycle; O-010 (manager lifetime/grammar approval) |
| NFR-01 | UC-01, UC-03 | AT-10 | Publishing and AOT: Target Matrix |
| NFR-02 | UC-01, UC-03 | AT-10 | Publishing and AOT: Decision Procedure |
| NFR-03 | UC-01, UC-03 | AT-10 | Publishing and AOT: Fallback and Release Gate |
| NFR-04 | UC-03, UC-05 | AT-02, AT-05 | Architecture: Logical Topology; Security: Management surface |
| NFR-05 | UC-05 | AT-05 | Architecture: Network and URL Model; Security: Sharing surface |
| NFR-06 | UC-01, UC-09 | AT-01, AT-04, AT-08, AT-14, AT-16 | Decisions: O-003, O-005, and O-007 |

## Traceability Gate

Before approval, verify that the matrix has exactly one row for every `FR-*` and `NFR-*`, each referenced `UC-*` and `AT-*` exists, and every design reference resolves to a section or `O-*` entry. Any requirement depending on an open decision must name that dependency and state whether it blocks implementation approval. Removed legacy flows must have a recorded decision and rationale.