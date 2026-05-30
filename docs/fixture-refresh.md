# Platform Fixture Refresh

Use this script when the platform dataset changes and the local test fixture needs to be regenerated.

## Source of truth

The refresh script reads the upstream dataset from:

`https://raw.githubusercontent.com/lancer1977/DataSeeds/master/retro/platform.json`

## Refresh command

From the repository root:

```bash
python scripts/refresh-platform-fixture.py
```

That writes the refreshed fixture to:

`API.SystemDefinitions.Test/TestData/platform.fixture.json`

## Dry run

Validate the dataset without writing any files:

```bash
python scripts/refresh-platform-fixture.py --dry-run
```

## Alternate paths

You can point the script at another dataset or output path when needed:

```bash
python scripts/refresh-platform-fixture.py \
  --source /path/to/platform.json \
  --output /tmp/platform.fixture.json
```

## Verification

After refreshing the fixture, run the focused test suite:

```bash
dotnet test API.SystemDefinitions.Test/API.SystemDefinitions.Test.csproj --no-restore
```

## Notes

- The refresh path is intentionally separate from NuGet publishing.
- Keep the fixture deterministic and JSON-only so the tests remain network-free.
