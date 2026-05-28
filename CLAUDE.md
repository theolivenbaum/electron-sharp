# CLAUDE.md

Guidance for Claude Code working in this repository.

## What this is

**ElectronSharp** is a hard fork of Electron.NET, maintained by @theolivenbaum and used to build the Curiosity app. It lets you build cross-platform desktop apps with .NET 10 + ASP.NET Core (Razor Pages, MVC, Blazor) by wrapping a Node/Electron host and bridging to it over a Socket.IO IPC channel.

- Public README: `README.md`
- Repo: https://github.com/theolivenbaum/electron-sharp
- Published NuGet packages: `ElectronSharp.API`, `ElectronSharp.CLI`

## Solution layout

| Project | Role |
|---|---|
| `ElectronSharp.API` | The .NET API surface (`Electron.*` static class, IPC client). Published as NuGet `ElectronSharp.API`. TFM `net10.0`. |
| `ElectronSharp.CLI` | `dotnet tool` named **`electron-sharp`**. Commands: `start`, `build`, `init`, `add`, `version`. Commands live in `ElectronSharp.CLI/Commands/`. The CLI ships the Host as embedded resources (see `EmbeddedFileHelper.cs`) and extracts them into the consumer project's `obj/Host/` at build time. |
| `ElectronSharp.Host` | The Electron-side Node.js app: `main.js` plus `api/*.js` modules. Talks to the .NET process over Socket.IO. **This is what gets embedded into the CLI package.** |
| `ElectronSharp.SampleApp` | Example ASP.NET Core app demonstrating usage. Not published. |
| `ElectronSharp.API.Tests` | xUnit tests. |
| `Releaser` | Maintainer-only console app for bumping the pinned Electron version. Run as `cd Releaser && dotnet run <x.y.z>`. |

## Where the Electron version is pinned

Three places, kept in sync by the `Releaser` project. Always use `Releaser` (or update all three) when bumping:

1. `ElectronSharp.Host/package.json` — `devDependencies.electron`
2. `ElectronSharp.CLI/Commands/BuildCommand.cs` — `_defaultElectronVersion` constant
3. `.devops/build-nuget.yaml` — `PackageVersion` variable (the NuGet package version is `<electron-version>.$(Build.BuildId)`, mirroring the Electron version)

`Releaser/Program.cs` uses three regexes that **require `x.x.x` form (1–2 digits per component)**. If you change the format, update the regexes too.

After updating, `Releaser` also runs `npm update -D` in `ElectronSharp.Host/`.

## Host (Node side)

- Entry: `ElectronSharp.Host/main.js`. Sets up the Electron app, spawns the ASP.NET Core executable, starts a Socket.IO server, authenticates the .NET client, and registers handlers from `api/*.js`.
- API modules in `ElectronSharp.Host/api/` mirror the Electron APIs the .NET wrapper exposes (app, browserWindows, dialog, menu, ipc, notification, tray, autoUpdater, etc.).
- Dependencies of note: `electron`, `electron-updater`, `socket.io`, `@electron/notarize`, `portscanner`, `image-size`.
- HostHook extensibility point: `ElectronSharp.Host/ElectronHostHook/` — consumer apps drop TypeScript in here to extend the host.
- Splash screen: `ElectronSharp.Host/splashscreen/`.

## CLI (build pipeline)

`ElectronSharp.CLI/Commands/BuildCommand.cs` orchestrates `dotnet publish` + `electron-builder`/`electron-packager`. Notable params: `/target`, `/electron-arch`, `/electron-version`, `/package-json`, `/install-modules`, `/Version`, `/p:Property=...`.

Embedded Host files are declared in `ElectronSharp.CLI/ElectronSharp.CLI.csproj` (lines ~43–72). Adding a new file to `ElectronSharp.Host/` means adding an `<EmbeddedResource>` entry, otherwise consumers won't see it.

## Build / test / pack

Local:

```bash
# Restore + build
dotnet build ElectronSharp.sln -c Release

# Test
dotnet test ElectronSharp.API.Tests/ElectronSharp.API.Tests.csproj

# Pack (output to ./artifacts)
dotnet pack ElectronSharp.API/ElectronSharp.API.csproj -c Release
dotnet pack ElectronSharp.CLI/ElectronSharp.CLI.csproj -c Release

# Install the locally built CLI tool
dotnet tool update --add-source ./artifacts/ -g electronsharp.cli
```

CI: `.devops/build-nuget.yaml` (Azure Pipelines). Builds with the .NET 9 SDK (currently — it still produces net10 output via SDK rolling-forward), packs, and pushes to the `nuget-curiosity-org` feed. The `PackageVersion` is sourced from the YAML, not from the csproj.

## TFMs and versions

- All projects: `net10.0`.
- NuGet csproj `<Version>` is `99.0.0.0` locally (a sentinel); the real version is injected by CI via `/property:Version=$(PackageVersion)`.

## Conventions

- C#: file-scoped or block namespaces under `ElectronSharp.*`; static `Electron` class is the public entry point; commands implement `ICommand` and are dispatched from `Program.cs`.
- No root `.editorconfig`. `.gitattributes` only sets line-ending normalization.
- TypeScript in `ElectronSharp.Host`: target ES2020, module commonjs. ESLint configured.
- No emojis in code or commits.

## Branch and commit etiquette

- This workspace operates on the branch declared in the system prompt — develop, commit, and push there.
- Don't open PRs unless asked.
- Don't include the model identifier in commits, PR text, or code.

## Common gotchas

- **Adding a new Host file?** Add it as an `<EmbeddedResource>` in `ElectronSharp.CLI.csproj` *and* make sure `EmbeddedFileHelper` extracts it (it iterates known files).
- **Bumping Electron?** Use `Releaser`. Don't hand-edit the three pinned locations or they drift.
- **Bumping electron-updater or other Host deps?** Edit `ElectronSharp.Host/package.json` and run `npm install` in that directory to refresh the lockfile — but the repo doesn't currently commit a lockfile, so consumers' `npm install` resolves at build time.
- **The .NET API and Host JS are tightly coupled.** When you change a Socket.IO message name on one side, change it on the other. Search both `ElectronSharp.API/` and `ElectronSharp.Host/api/` for the channel name.
