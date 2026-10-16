# Current State and Use Cases

## Evidence Rules

This inventory is based on `master` commit `fb7e33d`. The paths below identify read-only source evidence on that commit; they are not files to copy into v2. `Observed` describes existing code. `Candidate` identifies behavior to carry into requirements, subject to the decisions in [Decision Log](decisions.md).

## Existing Components

| Area | Observed behavior | v2 disposition |
| --- | --- | --- |
| WPF management UI | Manages multiple share tasks, a selected task, folder path, port, custom MIME mappings, start/stop, ordering, settings, links, QR views, and a bounded log list. | Port the interaction model, not WPF, XAML, MVVM-Sidekick, Windows APIs, or WPF-specific libraries. |
| ASP.NET Core 1.x file host | Serves a selected content root under `/files`, enables directory browsing, adds custom MIME mappings, and exposes MVC/API/Swagger routes. `Program.StartAsync` binds `0.0.0.0:<port>`. | Preserve the user-facing read/share flows with a .NET 10 host; revalidate old routes and security. |
| File API | Lists files and folders, returns the root path, and resolves cached short-link keys. | Preserve useful catalog behavior; do not expose a host's absolute filesystem path without an explicit decision. |
| WCF catalog | WPF starts a `ServiceHost`; a catalog tracks active hosts and supports `GetHosts` and `GetFolderDetail`. A second interface declares `UploadFile`, but the concrete service implements only the read/catalog interface. | Treat catalog behavior as a compatibility candidate. Do not infer that upload is implemented. Decide whether to replace WCF with HTTP or retire it. |
| Console host | Current `Program` prints its assembly path and waits for input; its host launch is commented out. | No functional sharing workflow to port from this entry point. |
| UWP app | The current page and view model are a title placeholder. | No functional sharing workflow to port from this entry point. |

### Legacy AOT conclusion

The existing `GreaterFileShare.Web` project is not a Native AOT-ready implementation to carry forward unchanged: it targets `net46`, uses MVC (`services.AddMvc()`), and the old ASP.NET Core pipeline/Swagger setup. The .NET 10 ASP.NET Core Native AOT compatibility table lists MVC as unsupported. v2 must reimplement the useful HTTP behavior on supported .NET 10 primitives and revalidate each dependency; changing only the target framework is not sufficient. See [ASP.NET Core support for Native AOT](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/native-aot?view=aspnetcore-10.0).

## Source Map

- WPF screens and commands: `GreaterFileShare.Hosts.WPF/MainWindow.xaml`, `ViewModels/MainWindow_Model.cs`, `Models/ShareFileTask.cs`.
- WPF link and file-browser flows: `ViewModels/UriAndQRs_Model.cs`, `Models/UrlGroup.cs`, `Models/HostEntry.cs`, `SimpleQR.xaml`, `ViewModels/SimpleQR_Model.cs`.
- WPF persistence and file selection: `Services/SettingRepoService.cs`, `Services/Win32SettingRepoService.cs`, `Services/ISettingRepoService.cs`, `Glue/PickFolderCommand.cs`, `Glue/PickFileCommand.cs`.
- WPF process/network registration: `App.xaml.cs`, `Services/NetworkService.cs`, `Services/FileSystemHubService.cs`.
- HTTP host and paths: `GreaterFileShare.Web/Program.cs`, `Startup.cs`, `Consts.cs`, `Controllers/FileApiController.cs`, `Controllers/ShortUriController.cs`, `Services/FileSystemService.cs`.
- WCF catalog: `GreaterFileShare.WCF/ISharingFileCatalogService.cs`, `ISharingFileCatalogService_2.cs`, `SharingFileCatalogWebSiteService.cs`.
- Public feature summary: root `README.md` at the source baseline.

## Observed Behavior Details

### Management and share tasks

- The management window has an ordered collection of share tasks and one current selection.
- A new task chooses port 8080 when the list is empty; otherwise it starts after the current maximum and searches for an unused port, wrapping to 5000 after 20000. The UI allows values from 0 through 65535. This is legacy behavior, not a v2 port policy.
- A task stores a path, nullable port, additional extension-to-content-type mappings, and an `IsSetToHosting` flag. The task reports start failure and its last exception.
- Starting constructs a content-type dictionary, ignores empty or duplicate entries with log messages, and starts the website host. Stop cancels that host.
- The WPF application tracks active hosts in the WCF catalog when task start/stop events occur. On application exit it stops hosted tasks and closes the WCF service host.
- When no settings exist, WPF creates one task and asks the platform storage service for a default Videos-library folder. That Windows-specific default needs a cross-platform product decision.
- On view load, previously saved tasks marked `IsSetToHosting` are started. Whether v2 should auto-start shares is unresolved because it can re-expose files after restart.

### File and directory selection

- WPF uses a native folder picker to set the selected task's root path.
- Settings import and export use native file pickers. The v2 UI must support a native folder picker and path entry/paste, with identical validation.
- The existing WPF browsing view displays files and folders under the selected task root. It supports selecting a host, file, or folder and refreshing the current folder.

### Links, QR codes, and host discovery

- A network service enumerates host names; the WPF flow filters `.local` names and sorts names with `192...` addresses first. This is not an adequate cross-platform interface-selection contract.
- For a selected host and task, the UI composes file-root, selected file/folder, short-link, Swagger API, and legacy WCF URLs.
- Short links use `/s/{key}` and redirect to the corresponding `/files/...` path. API routes are under `/api/FileApi` (route casing is framework-dependent). Swagger UI is under `/swagger/ui`.
- The UI has a share-links/QR view and a separate custom-URL QR view. The latter accepts a caller-provided URL.
- Link generation must account for IPv4, IPv6, selected listening interfaces, URL escaping, paths with spaces, and root-share links.

### Settings and logs

- The settings service uses `DataContractSerializer` for an `ObservableCollection<ShareFileTask>` and the `.gfssetting` extension. The model marks additional content types, path, port, and `IsSetToHosting` as serialized members.
- The default settings service uses a Documents-library location and a generated `Setting_{name}.gfssetting` name. A separate Win32 service accepts a selected file path for import/export. The exact byte-for-byte format, historical variants, and failure behavior still require fixture-based confirmation.
- Import replaces the in-memory task collection, stops current hosts first, and then starts tasks whose persisted flag requests hosting. Export writes the current collection. A command-line configuration path can be offered temporarily after a user prompt.
- The WPF management log list retains at most 500 entries. The file service has a process-local short-link cache with a nominal 5000-entry limit; short-link entries are not durable across restarts.

### HTTP file behavior and risks to address

- The content root is exposed through static file middleware and directory browsing below `/files`.
- Custom MIME mappings are applied to the static file content-type provider.
- The legacy file API accepts a folder path and checks `StartsWith(rootPath)` before enumerating. A string-prefix test is not a sufficient filesystem containment check; sibling-prefix paths, traversal forms, case handling, and links/reparse points need explicit security tests in v2.
- `GetRootPath` returns the absolute root path in the legacy API. v2 must decide whether to remove it or return a non-sensitive share-relative value.
- Error handling, port conflicts, permissions, inaccessible folders, and malformed MIME entries must be converted into visible task status rather than silently lost.

## Legacy Use Case Inventory

| ID | User flow | Source evidence | v2 status |
| --- | --- | --- | --- |
| UC-01 | Launch manager, load saved tasks, show a useful initial task when no settings exist. | `MainWindow_Model.OnBindedViewLoad`, `ShareFileTask` defaults. | Change: interim behavior pending O-003/O-007 approval; do not start sharing until the user chooses a root and explicitly starts it. |
| UC-02 | Create a share instance with a folder, port, and MIME overrides. | `CommandNewHost`, `ShareFileTask`, main window XAML. | Retain; add cross-platform selection and validation. |
| UC-03 | Start/stop one instance and see running, stopped, or failed state. | `ShareFileTask.Start/Stop`, task event/log handlers. | Retain; specify process and listener state transitions. |
| UC-04 | Run multiple instances, select one, reorder, or delete it. | `HostingTasks`, `CommandMoveItem`, `CommandDeleteHost`. | Retain; define port collision and deletion confirmation behavior. |
| UC-05 | Browse folders/files and open or copy a link to a share root, folder, or file. | `UriAndQRs_Model`, `UrlGroup`, HTTP static files. | Retain; define cross-platform links and access scope. |
| UC-06 | Generate QR codes for a selected share link or custom URL. | `CommandShowQR`, `CommandSimpleQR`, QR views. | Retain. |
| UC-07 | Override MIME types for uncommon extensions. | `AdditionalContentTypes`, `Startup.Configure`. | Retain with duplicate, invalid, and unsafe-value validation. |
| UC-08 | View recent application/host events and startup errors. | `Messages`, `WebLoggingSource`, task error state. | Retain with structured severity and per-instance correlation. |
| UC-09 | Save, export, import, and migrate sharing configuration. | `CommandSaveSettings`, `CommandExportSettings`, `CommandImportSettings`, setting services. | Retain; migration schema and rollback rules required. |
| UC-10 | Call file/folder catalog and short-link endpoints. | `FileApiController`, `ShortUriController`, `FileSystemService`. | Unresolved: decide v2 HTTP compatibility under O-004 before implementation. |
| UC-11 | Discover active hosts and query a folder catalog through WCF. | WPF `App.xaml.cs`, WCF service/contracts. | Unresolved: decide WCF replacement/retirement under O-004 before implementation. |
| UC-12 | Upload a file through the WCF v2 interface. | `ISharingFileCatalogService_2.UploadFile`. | Remove: no implementation observed; do not treat the declaration as a v2 upload requirement. |
| UC-13 | Start a host from the Console app. | Console `Program.cs`. | Remove: no implemented Console sharing flow to port. |
| UC-14 | Use UWP to manage or share files. | UWP `MainPage.xaml`, `MainPage_Model.cs`. | Remove: current UWP page is a placeholder, not a working sharing flow. |

## New V2 Use Cases

These are requested v2 behaviors, not reverse-engineered legacy behavior. Their requirements are defined in `requirements-and-acceptance.md`.

| ID | User flow | Source | v2 status |
| --- | --- | --- | --- |
| UC-15 | Edit a named profile in the UI, validate all share definitions, then save it for later use. | User-requested v2 behavior. | Added; profile schema and validation severity remain pending O-009. |
| UC-16 | Use a command-line `server` command with a saved profile to start and later stop its sharing instances. | User-requested v2 behavior. | Added; manager lifetime and final command grammar remain pending O-010. |

## Migration Mapping Rule

For each legacy UC-01 through UC-14, the requirements document must assign exactly one disposition: retain, change, remove, or unresolved. New v2 use cases are labeled `Added` and trace to requirements rather than being presented as legacy behavior. If a legacy flow has no complete implementation, label it as declared-but-unimplemented rather than working. Each retained or changed flow must have one or more acceptance tests.