# Decision Log

This log records user-confirmed decisions and unresolved design questions. `Accepted` means confirmed for the v2 documentation; it does not mean implementation has begun.

## Accepted

| ID | Decision | Basis |
| --- | --- | --- |
| D-001 | Write documentation only on the existing `v2` branch under `docs/v2/`; leave the mainline (`master` in this repository) unchanged. | User confirmation. |
| D-002 | Reverse-engineer all user-visible flows from the current repository; use WPF only as an interaction reference, not as a v2 technology dependency. | User confirmation. |
| D-003 | Target .NET 10 with a Blazor WebAssembly management UI served by the local manager web server; serve it through loopback-only Minimal API management endpoints. | User confirmation of the WASM + web-server mainline. |
| D-004 | Keep the management surface loopback-only and run the manager and file-sharing hosts as separate processes. | User confirmation. |
| D-005 | The manager starts and supervises host children; normal manager shutdown stops the children it owns. | User confirmation. |
| D-006 | Use line-delimited JSON-RPC 2.0 over stdio; stdout is protocol-only and diagnostics go to stderr. | User confirmation. |
| D-007 | A host defaults to every interface, explicitly including loopback; interface selection is editable before start and stored per share instance. | User confirmation. The UI must state that VPN/routed interfaces may expose the share beyond a LAN. |
| D-008 | Support both a native folder picker and typed/pasted paths with common validation. | User confirmation. |
| D-009 | Preserve settings import/export and investigate legacy migration. | User confirmation. |
| D-010 | Attempt to open the default browser at startup and print the loopback management URL. | User confirmation. |
| D-011 | Cover `win-x64`, `win-arm64`, `linux-x64`, `linux-arm64`, `osx-x64`, and `osx-arm64`; evaluate AOT per RID and use self-contained fallback per incompatible RID. | User confirmation. |
| D-012 | Require document traceability/consistency checks and explicit user approval before implementation. | User confirmation. |
| D-013 | Mainline sharing host: rewrite for .NET 10 using Minimal APIs for directory listings and short links, and StaticFiles for file downloads with a per-instance root. Do not carry the legacy MVC host forward or depend on `UseDirectoryBrowser` unless it passes AOT verification. | User-selected mainline; AOT compatibility remains a per-RID validation gate. |
| D-014 | The UI supports profile validation followed by saving a runnable, named profile. | User confirmed this product concept. Detailed schema, identity, storage, and validation rules remain pending O-009 and are required before implementation approval. |
| D-015 | A `server` CLI command accepts a saved profile and supports starting and stopping its sharing instances. | User confirmed this product concept. Exact grammar and manager lifecycle remain pending O-010 and are required before implementation approval. |

## Open Decisions for the Documentation Phase

| ID | Question | Required evidence or proposal |
| --- | --- | --- |
| O-001 | Which cross-platform native folder-picker integration meets the native-dialog requirement and AOT constraints? | Test candidate integrations on Windows, Linux, and macOS. If a picker prevents Native AOT on one RID, use self-contained for that manager/RID; do not silently replace the native-picker requirement with path entry. If no integration works on an OS, implementation remains blocked pending user approval of an alternative. |
| O-002 | Should the HTTP file share remain anonymous and read-only, or should a per-share credential be required? | Threat/risk comparison, especially because the default interface set includes VPN/routed interfaces. |
| O-003 | Should previously active shares auto-start after manager restart? | Interim implementation rule: no automatic start until approved. Recommendation: autostart disabled by default and enabled only by explicit per-profile opt-in; migration must not carry legacy `IsSetToHosting` into an active state silently. |
| O-004 | Which legacy APIs and WCF catalog behaviors remain compatible in v2? | Required before implementation approval of FR-11/AT-09. Map `FileApiController` file/folder/root/short-link cache routes, `ShortUriController` `/s/{key}`, Swagger, and WCF `GetHosts`/`GetFolderDetail`; explicitly exclude the unimplemented upload declaration unless newly approved. |
| O-005 | What exact legacy settings formats and versions must migrate? | Required before implementing AT-08. Inspect representative `.gfssetting` fixtures and confirm the `DataContractSerializer` collection shape, path variants, MIME entries, and `IsSetToHosting` behavior. Without real fixtures, migration remains unverified. |
| O-006 | How should port 0, automatically assigned ports, and port collisions behave? | The legacy UI accepted 0. Decide whether v2 supports OS-assigned ports; if so, return the actual bound port and generate links from it. Define duplicate bind-tuple handling, stability, persistence, and retry rules. |
| O-007 | What should the first share root be on each OS? | Recommendation: start with an empty, unshared profile requiring an explicit folder selection; do not silently expose a platform-specific directory. Approve a different cross-platform default only with explicit rationale. |
| O-008 | What does the end-user package contain and how is it signed? | Decide single-file/archive layout, host child location, permissions, macOS signing/notarization, and Windows signing separately from AOT status. |
| O-009 | What are the exact profile identity, storage, and validation rules? | Required before implementing FR-15/AT-11. Recommendation: versioned JSON, UUID profile/instance IDs, unique user-facing profile name, ordered instances. Blocking errors: invalid schema, missing/inaccessible root, out-of-range or conflicting bind tuple, duplicate IDs, malformed MIME. Warnings: temporarily unavailable interface and exposure beyond private LAN. Final rules require user review. |
| O-010 | Approve CLI manager lifecycle and final command grammar. | `server` profile start/stop is user-requested. Recommended lifecycle: start reuses or launches a background headless manager and returns after readiness; stop stops only the profile; `server shutdown` stops all managed hosts and exits. Exact syntax, handoff, and daemon lifetime remain pending user review; AT-13 is conditional on approval. |
| O-011 | What is the default symlink/reparse-point policy? | Security recommendation: reject all links by default, including in-root links, until a cross-platform policy is approved; always reject targets outside root. AT-06 must cover Windows reparse points and Unix symlinks. |
| O-012 | What happens to host children after an abnormal manager exit? | Recommendation: child exits when the parent's stdio control channel closes, preventing orphaned shares. Verify on all target OSes and document forced-termination fallback. |

## Decision Closure Rule

Each open item must become one of:

- A documented recommendation with evidence and explicit user approval.
- A user-confirmed requirement.
- A deferred item marked as a blocker for the affected implementation scope.

No open item may be silently resolved by implementation convenience. A new user-facing scope change requires approval before code work.