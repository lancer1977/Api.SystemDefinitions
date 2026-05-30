# Release Checklist

## Package Metadata

- [x] Package ID is `PolyhydraGames.Api.SystemDefinitions`.
- [x] Repository URL points to `https://github.com/lancer1977/Api.SystemDefinitions`.
- [x] README and LICENSE are packed into the NuGet package.
- [x] Target framework is documented as `.NET 10`.

## Tests

- [x] Default tests use `API.SystemDefinitions.Test/TestData/platform.fixture.json`.
- [x] Default tests avoid live remote dataset calls.
- [x] Alias normalization is covered.
- [x] Extension lookup empty behavior is covered.

## CI

- [x] GitHub Actions restores `Api.SystemDefinitions.sln`.
- [x] GitHub Actions builds the solution.
- [x] GitHub Actions runs deterministic tests.
- [x] GitHub Actions packs the library and uploads the package artifact.

## Future Work

- [ ] Add a release workflow for NuGet publishing.
- [ ] Add generated API reference documentation.
- [x] Add a fixture update script for refreshing the platform dataset.
