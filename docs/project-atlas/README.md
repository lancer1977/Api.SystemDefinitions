# Api.SystemDefinitions Project Atlas

## Purpose

`Api.SystemDefinitions` provides deterministic platform definitions for retro game inventory and catalog lookup work. It maps aliases, folders, emulator cores, file extensions, provider identifiers, and ROM paths to canonical system slugs.

## Primary Surfaces

- Runtime library: `Api.SystemDefinitions/SystemsDatabase.cs` and `Api.SystemDefinitions/SystemHelpers.cs`.
- Package metadata: `Api.SystemDefinitions/PolyhydraGames.Api.SystemDefinitions.csproj`.
- Fixture-backed tests: `API.SystemDefinitions.Test/SystemsTests.cs`.
- Curated test fixture: `API.SystemDefinitions.Test/TestData/platform.fixture.json`.
- Fixture refresh helper: `scripts/refresh-platform-fixture.py`.

## Mapping Contract

Consumers should treat `SystemDefinition.Slug` as the canonical platform key and `SystemDefinition.Folder` as the Batocera/local collection folder key. Provider-specific IDs are resolved through `GetProviderIdFromSlug`; the current curated dataset exposes IGDB IDs and returns an empty string for providers that are not present yet.

## Validation

```bash
dotnet restore Api.SystemDefinitions.sln
dotnet test Api.SystemDefinitions.sln --no-restore
dotnet build Api.SystemDefinitions.sln --no-restore
dotnet pack Api.SystemDefinitions/PolyhydraGames.Api.SystemDefinitions.csproj --configuration Release --no-restore --output ./artifacts/package
dotnet list Api.SystemDefinitions.sln package --outdated
devstudio validate --repo .
```

Fixture tests should run without network access. Use the refresh script only when intentionally updating the curated platform dataset.
