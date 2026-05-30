#!/usr/bin/env python3
"""Refresh the local platform fixture from the canonical dataset.

The default source is the upstream platform dataset used by SystemsDatabase.
The script validates the JSON shape before writing the fixture file so the
refresh path is safe to run as a manual maintenance step.
"""

from __future__ import annotations

import argparse
import json
from pathlib import Path
from typing import Any
from urllib.parse import urlparse
from urllib.request import urlopen

DEFAULT_SOURCE = "https://raw.githubusercontent.com/lancer1977/DataSeeds/master/retro/platform.json"
DEFAULT_OUTPUT = Path(__file__).resolve().parents[1] / "API.SystemDefinitions.Test" / "TestData" / "platform.fixture.json"
REQUIRED_FIELDS = ("IgdbId", "Name", "Folder", "Slug", "Extensions", "Core")


def parse_args() -> argparse.Namespace:
    parser = argparse.ArgumentParser(
        description="Refresh API.SystemDefinitions.Test/TestData/platform.fixture.json from the upstream platform dataset.",
    )
    parser.add_argument(
        "--source",
        default=DEFAULT_SOURCE,
        help=f"Dataset source to read from (default: {DEFAULT_SOURCE})",
    )
    parser.add_argument(
        "--output",
        default=str(DEFAULT_OUTPUT),
        help=f"Fixture file to write (default: {DEFAULT_OUTPUT})",
    )
    parser.add_argument(
        "--dry-run",
        action="store_true",
        help="Validate the source dataset without writing the output file.",
    )
    return parser.parse_args()


def read_source(source: str) -> str:
    parsed = urlparse(source)
    if parsed.scheme in {"http", "https"}:
        with urlopen(source) as response:  # nosec: B310 - deliberate maintenance fetch from trusted source
            return response.read().decode("utf-8")

    if parsed.scheme == "file":
        path = Path(parsed.path)
    else:
        path = Path(source)

    return path.read_text(encoding="utf-8")


def load_systems(source: str) -> list[dict[str, Any]]:
    raw = read_source(source)
    data = json.loads(raw)

    if not isinstance(data, list):
        raise ValueError("Expected the dataset to be a JSON array of system definitions.")

    systems: list[dict[str, Any]] = []
    for index, item in enumerate(data):
        if not isinstance(item, dict):
            raise ValueError(f"Item {index} is not an object.")

        missing = [field for field in REQUIRED_FIELDS if field not in item]
        if missing:
            raise ValueError(f"Item {index} is missing required field(s): {', '.join(missing)}")

        systems.append(item)

    return systems


def write_fixture(path: Path, systems: list[dict[str, Any]]) -> None:
    path.parent.mkdir(parents=True, exist_ok=True)
    text = json.dumps(systems, indent=2, ensure_ascii=False) + "\n"
    path.write_text(text, encoding="utf-8")


def main() -> int:
    args = parse_args()
    systems = load_systems(args.source)
    output = Path(args.output)

    if args.dry_run:
        print(f"Validated {len(systems)} system definitions from {args.source}; no files written.")
        return 0

    write_fixture(output, systems)
    print(f"Wrote {len(systems)} system definitions to {output}")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
