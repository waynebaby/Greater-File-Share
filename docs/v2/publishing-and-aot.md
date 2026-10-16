# Publishing and AOT

## Target Matrix

Every target is evaluated independently. Initial status is `Not evaluated`; no AOT support is implied by this document.

| OS | RID | Architecture | Manager executable Native AOT | Share-host executable Native AOT | Fallback |
| --- | --- | --- | --- | --- | --- |
| Windows | `win-x64` | x64 | Not evaluated; WASM UI + Minimal API server | Not evaluated; Minimal API + StaticFiles | Per executable: self-contained |
| Windows | `win-arm64` | arm64 | Not evaluated; WASM UI + Minimal API server | Not evaluated; Minimal API + StaticFiles | Per executable: self-contained |
| Linux | `linux-x64` | x64 | Not evaluated; WASM UI + Minimal API server | Not evaluated; Minimal API + StaticFiles | Per executable: self-contained |
| Linux | `linux-arm64` | arm64 | Not evaluated; WASM UI + Minimal API server | Not evaluated; Minimal API + StaticFiles | Per executable: self-contained |
| macOS | `osx-x64` | x64 | Not evaluated; WASM UI + Minimal API server | Not evaluated; Minimal API + StaticFiles | Per executable: self-contained |
| macOS | `osx-arm64` | arm64 | Not evaluated; WASM UI + Minimal API server | Not evaluated; Minimal API + StaticFiles | Per executable: self-contained |

Blazor WebAssembly client AOT is tracked separately: it produces WebAssembly for the browser, not one native executable per RID. Record its build-tools workload, publish result, size, and browser smoke-test evidence in a separate row/section. The mainline is selected; this client check does not replace the native manager/host matrix.

The WebAssembly AOT result is a release-time check, not a prerequisite for approving the pre-implementation documents. Record it once per version-matched manager/WASM client artifact, not once per native RID. If it fails, use the documented non-AOT WebAssembly client fallback; manager and share-host Native AOT results remain independent.

The manager packages the version-matched Blazor WebAssembly assets as its local static content; client and manager versions move together. If WebAssembly AOT fails or is unsupported, publish the standard non-AOT Blazor WebAssembly client for that manager build. This does not change the independent Native AOT or self-contained result for the manager and share-host executables. Do not label the whole package simply "AOT"; report WASM client AOT and each native executable/RID separately.

### Selected Mainline: WebAssembly UI plus Minimal API/StaticFiles servers

This arrangement can use AOT in both layers:

- **Browser UI:** enable Blazor WebAssembly AOT (`RunAOTCompilation`) with the .NET 10 `wasm-tools` workload. The result is WebAssembly and browser assets, not a Windows, Linux, or macOS executable.
- **Manager web server:** publish the manager as Native AOT separately for each native RID. Serve the WASM app as static assets and keep management operations in loopback-only Minimal APIs. Microsoft lists StaticFiles as supported, Minimal APIs as partially supported, and Blazor Server/MVC as unsupported for ASP.NET Core Native AOT in the current .NET 10 compatibility table.
- **Share host:** use Minimal APIs for directory listing/short links and StaticFiles for downloads with a per-instance physical file provider. Independently evaluate the share host for Native AOT on each native RID.

Thus, the answer is "yes, conditionally" for a WebAssembly client and "potentially" for each native server executable. WASM AOT does not compensate for a non-AOT-compatible server feature. Interactive Server is not part of the selected mainline; if later reintroduced, its manager must use self-contained publishing under the current compatibility table.

The folder picker is also a separate compatibility check: browser APIs cannot give the server unrestricted access to an arbitrary local absolute path. To preserve the native-picker requirement, the local manager must invoke a platform-native picker and return the chosen path through its loopback API. Verify the chosen native integration on all three operating systems and under AOT. If a picker library prevents Native AOT only on a particular RID, fall back to self-contained for that manager/RID; do not silently remove the native-picker requirement. If no integration works on an OS, keep that scope blocked pending user approval of an alternative under O-001.

## Current .NET 10 Support Evidence

The [ASP.NET Core Native AOT compatibility table](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/native-aot?view=aspnetcore-10.0) currently lists Blazor Server and MVC as not supported, Minimal APIs as partially supported, StaticFiles as supported, and SignalR as partially supported. Therefore:

- If the manager uses Interactive Server, mark manager Native AOT as unsupported by the framework and use self-contained publishing for that executable on all RIDs. Do not spend the matrix on repeated attempts to bypass a declared framework limitation.
- Continue evaluating the share-host executable independently for Native AOT compatibility.
- If the manager uses a WebAssembly UI with loopback Minimal APIs, treat manager Native AOT as a candidate requiring publish/runtime validation; Minimal APIs are only partially supported.
- Blazor WebAssembly AOT is a distinct browser compilation mode. Microsoft documents the `RunAOTCompilation` property and the `wasm-tools` workload for .NET 10; this does not replace Native AOT evaluation of manager/host executables. See [Blazor WebAssembly AOT compilation](https://learn.microsoft.com/en-us/aspnet/core/blazor/webassembly-build-tools-and-aot?view=aspnetcore-10.0).

### File API and directory browsing

The existing Web project cannot be carried forward unchanged as Native AOT: it targets `net46` and uses MVC, which is listed as unsupported. A .NET 10 rewrite using Minimal APIs for JSON endpoints and StaticFiles for downloads is a Native AOT candidate, not a blanket guarantee. Minimal APIs are only partially supported and their JSON payload types require source-generated serialization metadata.

The current share host also uses `UseDirectoryBrowser`, but the selected mainline does not depend on it. The official feature table classifies StaticFiles but does not give DirectoryBrowser a separate compatibility status. Do not infer support merely because both APIs are in `Microsoft.AspNetCore.StaticFiles.dll. Implement directory listings with explicit Minimal API behavior. If there is a reason to retain `UseDirectoryBrowser`, first publish and exercise it for every target; it is not the default design.

## Decision Procedure

For each RID:

1. Select the Blazor hosting mode only after checking official .NET 10 Native AOT support and the known Blazor Server limitation above.
2. Evaluate manager and share-host executables independently. Record SDK version, host OS/toolchain, exact command, warnings, and output artifacts for each RID.
3. If using Blazor WebAssembly AOT, separately record the `wasm-tools` workload, `RunAOTCompilation` result, client bundle size, and browser tests.
4. Run published native artifacts on the target OS/architecture. Verify browser startup, loopback-only admin, stdio JSON-RPC, child lifecycle, file serving, native folder picker, and settings operations.
5. If AOT is unsupported, the toolchain is unavailable, or required behavior fails for an AOT-specific reason, publish that executable/RID self-contained and repeat the same runtime tests.
6. Do not mark a RID complete until every required executable runs without an installed .NET runtime and the test evidence is attached or summarized.

## Compatibility Review

Review all dependencies and code paths for:

- Dynamic code, reflection-based serialization, trimming warnings, runtime code generation, and unsupported APIs.
- Blazor render mode, Razor compilation, static assets, WebSocket/SignalR requirements if applicable, and any native UI picker integration.
- JSON-RPC serialization and generated metadata; prefer source-generated serialization where required by AOT.
- QR generation, MIME handling, cryptography, directory enumeration, IPv4/IPv6 binding, and process launch/wait behavior.
- Platform-specific native dependencies and prerequisites for Native AOT toolchains.
- Blazor Server's documented Native AOT exclusion; do not treat self-contained JIT publishing as Native AOT.
- For WebAssembly AOT, the separate browser build tools and the larger client bundle tradeoff.

Warnings must be classified, not suppressed wholesale. Any suppression requires a rationale and a runtime test covering the affected path.

## Artifact Contract

- Produce a separate artifact per RID. The manager and share host must be packaged together or the manager must locate its matching host reliably without depending on the current working directory.
- Document executable names, configuration location, content/static assets, log location, file permissions, and first-run behavior.
- `Self-contained` means the .NET runtime is included. It does not by itself guarantee a single-file package; single-file and archive layout are separate decisions.
- Windows artifacts should have the expected executable extension. Linux/macOS launch permissions and macOS signing/notarization requirements are release decisions, not assumed prerequisites for local testing.
- Do not publish a mixed-architecture bundle under one ambiguous name.

## Command Templates

The final project paths and supported properties must be set after the project structure is approved. Expected command shapes are:

```powershell
dotnet publish <ManagerProject> -c Release -r <RID> --self-contained true -p:PublishAot=true
dotnet publish <ManagerProject> -c Release -r <RID> --self-contained true -p:PublishAot=false
```

Use the first command only for an executable whose framework features support Native AOT. For an Interactive Server manager, the fallback command is the expected route on every RID under the current support table. Otherwise choose fallback only for the executable/RID that fails the gate. If the host is separately published, record its matching command and RID as well. WebAssembly AOT uses its client workload/property and is not represented by either native executable command.

## Release Gate

The release matrix is complete only when all six rows record one of `AOT verified`, `self-contained verified`, or `blocked` with a reason and owner. A blocked target cannot be silently omitted. The user approves any scope change to the matrix.