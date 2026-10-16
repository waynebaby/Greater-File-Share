# Security and Testing

## Security Boundaries

### Management surface

- Bind the management UI and any manager API exclusively to loopback addresses. Verify both IPv4 and IPv6 loopback behavior.
- Do not bind a management listener to wildcard addresses. Do not proxy management routes through a share host.
- Treat loopback as a network boundary, not authentication. Define Host/Origin validation, CSRF protection for state-changing requests, and safe browser behavior before implementing a local HTTP management API.
- The manager-to-host JSON-RPC channel is stdio only. Keep stdout protocol-only and avoid exposing control methods on the share listener.

### Sharing surface

- The confirmed default binds all interfaces, including loopback. A per-instance interface selector can narrow this set before start. The UI must list the exact selected adapters/addresses and call out VPN/routed interfaces because they may extend access beyond a local LAN.
- No public exposure should be implied by the phrase "LAN sharing". If a selected address is reachable from a routed/public network, the share may be reachable there unless the OS firewall or a later authorization feature prevents it.
- The legacy static-file behavior is read-only. The WCF `UploadFile` declaration is not evidence of a working upload path; do not add write access as an accidental compatibility feature.
- Decide whether sharing is anonymous and read-only or requires a per-share credential before implementation. The legacy HTTP paths appear unauthenticated; preserving that behavior is a product decision with greater risk under all-interface binding.

### Filesystem boundary

- Resolve the configured root once to a canonical absolute path and verify it exists and is accessible.
- Resolve every requested relative path against that root and verify the canonical result remains within the root using path-aware comparison, not string-prefix comparison.
- Conservative recommendation pending O-011: do not follow symlinks or Windows reparse points, including links whose target is inside the share root. Always reject targets outside the root. If a later policy permits links, resolve the final target and verify containment using race-resistant operations.
- Reject encoded traversal, alternate separators, null/control characters, invalid device paths, and platform-specific path edge cases.
- Do not disclose the absolute root path in external responses or errors.

### Settings and logs

- Import must be treated as untrusted input: bound file size/depth, validate ports, interfaces, MIME values and paths, reject unknown dangerous fields, and apply transactionally.
- Settings exports must never contain credentials in plaintext. If O-002 approves credentials, export must omit them by default or use an explicitly approved protected format.
- Avoid logging secrets or full sensitive paths. If paths are necessary for local diagnostics, provide a redaction policy and keep them out of shared HTTP responses.

## Verification Plan

| Layer | Verification |
| --- | --- |
| Document consistency | Validate local links, legacy/new use-case classifications, requirement/use-case/test/decision traceability, RID names, approved versus open decisions, and consistent bind/process rules. |
| Unit | Validate configuration parsing/migration, path containment, URL generation/escaping, MIME mappings, interface selection, JSON-RPC framing, and state transitions. |
| Process integration | Launch manager and a host child; exercise JSON-RPC start/status/stop; simulate malformed frames, stderr volume, child crash, port conflict, and parent shutdown. |
| HTTP integration | Verify root listing, nested listing, file download, content type, short-link resolution, API responses, error behavior, and absence of management endpoints. |
| Network integration | Probe admin endpoints over loopback and non-loopback; probe each selected and deselected host address including loopback, IPv4, IPv6, and multiple adapters. |
| Settings integration | After O-005 identifies supported legacy fixtures, test migration; verify legacy hosting flags do not start hosts without user action (AT-16); import/export round-trip; corrupt/truncated input; ensure failed import preserves the previous file and running configuration. |
| Cross-platform | Run the same smoke scenarios for all six RIDs, using AOT or self-contained output as recorded in the publishing matrix. |
| Manual UX | Verify native folder selection, path input, browser auto-open/manual URL, per-instance interface selection, links/QR, and visible warnings on Windows/macOS/Linux. |

## Documentation Validation Checklist

- [x] Every source-backed observation points to a source path and the pinned baseline commit.
- [x] Every legacy use case has a disposition; new v2 use cases are separately labeled and traced. Retained/changed cases have acceptance criteria.
- [x] Each requirement traces to an acceptance test and architecture section; dependencies on open decisions are named.
- [x] Loopback-only admin behavior and its non-loopback refusal test are specified; runtime proof remains for implementation validation.
- [x] The all-interface default, loopback inclusion, interface changes, and VPN/routed exposure are stated consistently.
- [x] Filesystem containment tests address path-prefix and symlink/reparse-point escapes according to O-011.
- [x] Migration, failed import rollback, and unknown legacy fields have explicit behavior or are clearly blocked on O-005 fixtures.
- [x] All six RID evaluation methods, framework restrictions, and self-contained fallbacks are documented; actual publish results remain a release-time check.
- [x] Open decisions remain visible and are not presented as approved behavior.
- [x] All local Markdown links resolve and no Markdown file has trailing whitespace.
- [ ] The user reviews and approves the complete document set.

Passing this checklist validates the documents only. It does not authorize implementation until the user explicitly approves the reviewed specifications.