# Api.SystemDefinitions Project Atlas

## Purpose

`Api.SystemDefinitions` provides deterministic platform definitions for retro game inventory and catalog lookup work. It maps aliases, folders, emulator cores, file extensions, IGDB identifiers, and ROM paths to canonical system slugs.

## Primary Surfaces

- Runtime library: `Api.SystemDefinitions/SystemsDatabase.cs` and `Api.SystemDefinitions/SystemHelpers.cs`.
- Package metadata: `Api.SystemDefinitions/PolyhydraGames.Api.SystemDefinitions.csproj`.
- Fixture-backed tests: `API.SystemDefinitions.Test/SystemsTests.cs`.
- Curated test fixture: `API.SystemDefinitions.Test/TestData/platform.fixture.json`.
- Fixture refresh helper: `scripts/refresh-platform-fixture.py`.

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
