# PolyhydraGames.Api.SystemDefinitions

Typed system definition helpers for game platform slugs, folders, emulator
cores, file extensions, and IGDB identifiers.

The library wraps a curated platform dataset and normalizes common aliases such
as `mastersystem` -> `sms`, `pce` -> `pcengine`, and arcade variants -> `arcade`.

## Tags

- api
- dotnet
- api-system-definitions
- system
- definitions
- streaming

## Usage

```csharp
await SystemsDatabase.Setup(logger, dataSource: "path/to/platform.fixture.json");

var system = SystemsDatabase.Instance.GetSystem("SMS");
var folder = "mastersystem".ToFolder();
var core = "SMS".GetCoreFromSlug();
var slug = SystemsDatabase.Instance.GetSystemFromExtension(".gba");
var inventorySlug = SystemsDatabase.Instance.GetSystemFromPath("/roms/snes/Chrono Trigger.sfc");
var igdbPlatformId = SystemsDatabase.Instance.GetProviderIdFromSlug("snes", "igdb");
```

## Tests

Default tests use a local fixture and should not require network access.

Use `python scripts/refresh-platform-fixture.py` to regenerate `API.SystemDefinitions.Test/TestData/platform.fixture.json` from the upstream dataset when the fixture needs a maintenance refresh.

```bash
bash scripts/validate.sh
```

## Build And Pack

```bash
dotnet build Api.SystemDefinitions.sln --no-restore
dotnet pack Api.SystemDefinitions/PolyhydraGames.Api.SystemDefinitions.csproj --configuration Release --no-restore
```

## Roadmap

- [v1 roadmap](docs/roadmap/v1/README.md)
- [Platform fixture refresh guide](docs/fixture-refresh.md)
- [Release checklist](docs/release-checklist.md)

## Docs

- [Docs Index](./docs/README.md)
- [Feature Index](./docs/features/README.md)
- [Roadmap Index](./docs/roadmaps/README.md)

## License

This project is licensed under the MIT License.
